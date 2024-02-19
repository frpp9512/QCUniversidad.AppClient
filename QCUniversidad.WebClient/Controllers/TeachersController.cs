using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Models.Teachers;
using QCUniversidad.WebClient.Services.Contracts;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Auth")]
public class TeachersController(ITeachersDataProvider teachersDataProvider,
                                ISchoolYearDataProvider schoolYearDataProvider,
                                IDepartmentsDataProvider departmentsDataProvider,
                                IDisciplinesDataProvider disciplinesDataProvider,
                                IMapper mapper,
                                IOptions<NavigationSettings> navOptions,
                                ILogger<TeachersController> logger,
                                IExcelParser<TeacherModel> teachersExcelParser) : Controller
{
    private readonly ITeachersDataProvider _teachersDataProvider = teachersDataProvider;
    private readonly ISchoolYearDataProvider _schoolYearDataProvider = schoolYearDataProvider;
    private readonly IDepartmentsDataProvider _departmentsDataProvider = departmentsDataProvider;
    private readonly IDisciplinesDataProvider _disciplinesDataProvider = disciplinesDataProvider;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<TeachersController> _logger = logger;
    private readonly IExcelParser<TeacherModel> _teachersExcelParser = teachersExcelParser;
    private readonly NavigationSettings _navigationSettings = navOptions.Value;

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page = 0)
    {
        _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
        try
        {
            _logger.LogInformation($"Loading total teachers count.");
            int total = await _teachersDataProvider.GetTeachersCountAsync();
            _logger.LogInformation("Exists {0} teachers in total.", total);
            int pageIndex = page - 1 < 0 ? 0 : page - 1;
            int startingItemIndex = pageIndex * _navigationSettings.ItemsPerPage;
            if (startingItemIndex < 0 || startingItemIndex >= total)
            {
                startingItemIndex = 0;
            }

            _logger.LogInformation($"Loading teachers starting in {startingItemIndex} and taking {_navigationSettings.ItemsPerPage}.");
            IList<TeacherModel> teachers = await _teachersDataProvider.GetTeachersAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
            _logger.LogInformation($"Loaded {teachers.Count} teachers.");
            int totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
            NavigationListViewModel<TeacherModel> viewModel = new()
            {
                Items = teachers,
                CurrentPage = pageIndex + 1,
                PagesCount = totalPages,
                TotalItems = total
            };
            _logger.LogInformation("Returning view with {0} teachers, in page {1}, total pages: {2}, total items: {3}", viewModel.ItemsCount, viewModel.CurrentPage, viewModel.PagesCount, viewModel.TotalItems);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Exception throwed {0}", ex.Message);
            return RedirectToActionPermanent("Error", "Home");
        }
    }

    [Authorize("Admin")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> DetailsAsync(Guid id, string returnTo = "/")
    {
        try
        {
            TeacherModel teacher = await _teachersDataProvider.GetTeacherAsync(id);
            Models.SchoolYears.SchoolYearModel currentSchoolYear = await _schoolYearDataProvider.GetCurrentSchoolYear();
            IList<Models.Periods.PeriodModel>? periods = currentSchoolYear.Periods;
            ViewData["schoolYear"] = currentSchoolYear;
            ViewData["returnTo"] = returnTo;
            return View(teacher);
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> ImportAsync()
    {
        IList<Models.Departments.DepartmentModel> departments = await _departmentsDataProvider.GetDepartmentsAsync();
        ViewData["departments-list"] = departments;
        return View();
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportAsync(IFormFile formFile, Guid selectedDepartment)
    {
        if (formFile is null)
        {
            TempData["importing-error"] = "Debes de seleccionar un fichero para importar";
            return RedirectToAction("Import");
        }
        Stream fileStream = formFile.OpenReadStream();
        IList<TeacherModel> parsedModels = await GetParsedModelsAsync(fileStream);
        if (!parsedModels.Any())
        {
            TempData["importing-error"] = "No se ha podido importar ningún profesor.";
        }

        int created = 0;
        int updated = 0;
        int failed = 0;
        foreach (TeacherModel? newTeacher in parsedModels.Where(t => t.ImportAction == TeacherImportAction.Create))
        {
            try
            {
                newTeacher.DepartmentId = selectedDepartment;
                _ = await _teachersDataProvider.CreateTeacherAsync(newTeacher);
                created++;
            }
            catch
            {
                failed++;
            }
        }

        foreach (TeacherModel? teacherToUpdate in parsedModels.Where(t => t.ImportAction == TeacherImportAction.Update))
        {
            try
            {
                TeacherModel teacher = await _teachersDataProvider.GetTeacherAsync(teacherToUpdate.PersonalId);
                teacher.DepartmentId = selectedDepartment;
                teacher.Fullname = teacherToUpdate.Fullname;
                teacher.Position = teacherToUpdate.Position;
                teacher.Category = teacherToUpdate.Category;
                teacher.Email = teacherToUpdate.Email;
                teacher.ContractType = teacherToUpdate.ContractType;
                _ = await _teachersDataProvider.UpdateTeacherAsync(teacher);
            }
            catch
            {
                failed++;
            }
        }

        TempData["importing-result"] = $"Se han importado un total de {created + updated} profesores, creando {created} y actualizando {updated}, con {failed} fallos.";
        return RedirectToAction("Index");
    }

    [Authorize("Admin")]
    [HttpPost]
    public async Task<IActionResult> ImportFilePreviewAsync(IFormFile formFile)
    {
        Stream fileStream = formFile.OpenReadStream();
        IList<TeacherModel> parsedModels = await GetParsedModelsAsync(fileStream);
        return Json(parsedModels);
    }

    private async Task<IList<TeacherModel>> GetParsedModelsAsync(Stream fileStream)
    {
        IList<TeacherModel> parsedModels = await _teachersExcelParser.ParseExcelAsync(fileStream);
        foreach (TeacherModel parsedModel in parsedModels)
        {
            TeacherImportAction action;
            if (!ValidatePersonalId(parsedModel.PersonalId))
            {
                action = TeacherImportAction.NoImport;
            }
            else
            {
                bool exists = await _teachersDataProvider.ExistsTeacherAsync(parsedModel.PersonalId);
                action = exists ? TeacherImportAction.Update : TeacherImportAction.Create;
            }

            parsedModel.ImportAction = action;
        }

        return parsedModels;
    }

    public async Task<IActionResult> TemplateFileAsync()
    {
        if (!System.IO.File.Exists("templates/teachers_import.xlsx"))
        {
            return NotFound("The template file is missing!");
        }

        byte[] templateBytes = await System.IO.File.ReadAllBytesAsync("templates/teachers_import.xlsx");
        return File(templateBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QCU Plantilla para importar profesores.xlsx");
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
        CreateTeacherModel viewmodel = new();
        await LoadCreateViewModel(viewmodel);
        return View(viewmodel);
    }

    private async Task LoadCreateViewModel(CreateTeacherModel model)
    {
        await LoadDisciplinesIntoCreateModel(model);
        IList<Models.Departments.DepartmentModel> departments = await _departmentsDataProvider.GetDepartmentsAsync();
        model.DepartmentList = departments;
    }

    private async Task LoadDisciplinesIntoCreateModel(CreateTeacherModel model)
    {
        IList<DisciplineModel> disciplines = await _disciplinesDataProvider.GetDisciplinesAsync();
        model.Disciplines = disciplines;
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(CreateTeacherModel model)
    {
        _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
        if (ModelState.IsValid)
        {
            _logger.LogInformation($"Validating personal id {model.PersonalId}");
            if (!ValidatePersonalId(model.PersonalId))
            {
                _logger.LogWarning($"The personal id {model.PersonalId} is invalid.");
                ModelState.AddModelError("Error carné identidad.", "El carné de identidad no esta escrito correctamente.");
            }
            else
            {
                _logger.LogInformation($"Validating department existence with id {model.DepartmentId}.");
                if (await _departmentsDataProvider.ExistsDepartmentAsync(model.DepartmentId))
                {
                    _logger.LogInformation($"Validating selected disciplines existence.");
                    if (model.SelectedDisciplines is null || await CheckDisciplinesExistence(model.SelectedDisciplines))
                    {
                        _logger.LogInformation($"Requesting create new teacher.");
                        model.Disciplines ??= new List<DisciplineModel>(model.SelectedDisciplines?.Select(id => new DisciplineModel { Id = id, Name = "" }) ?? new List<DisciplineModel>());
                        bool result = await _teachersDataProvider.CreateTeacherAsync(model);
                        if (result)
                        {
                            _logger.LogInformation("The teacher was created successfully.");
                            TempData["teacher-created"] = true;
                            return RedirectToActionPermanent("Index");
                        }

                        _logger.LogError("Error while creating teacher.");
                        ModelState.AddModelError("Error creating teacher", "Ha ocurrido un error mientras se creaba el profesor.");
                    }
                    else
                    {
                        _logger.LogWarning($"At least one selected discipline does not exist.");
                        ModelState.AddModelError("Error", "Al menos una de las disciplinas seleccionadas no existe.");
                    }
                }
                else
                {
                    _logger.LogWarning($"The department with id {model.DepartmentId} does not exist.");
                    ModelState.AddModelError("Error departamento", "El departamento seleccionado no existe.");
                }
            }
        }

        await LoadCreateViewModel(model);
        return View(model);
    }

    private async Task<bool> CheckDisciplinesExistence(Guid[] disciplinesIds)
    {
        foreach (Guid id in disciplinesIds)
        {
            if (!await _disciplinesDataProvider.ExistsDisciplineAsync(id))
            {
                return false;
            }
        }

        return true;
    }

    private bool ValidatePersonalId(string personalId)
    {
        return (personalId.Length == 11)
            && personalId.Any(char.IsNumber)
            && (int.Parse(personalId.Substring(2, 2)) <= 12)
            && (int.Parse(personalId.Substring(4, 2)) <= 31);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> EditAsync(Guid id)
    {
        _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
        _logger.LogInformation($"Checking if the teacher with id {id} exists.");
        if (await _teachersDataProvider.ExistsTeacherAsync(id))
        {
            TeacherModel teacher = await _teachersDataProvider.GetTeacherAsync(id);
            teacher.SelectedDisciplines = teacher.Disciplines?.Select(d => d.Id).ToArray();
            teacher.Disciplines?.Clear();
            await LoadEditViewModel(teacher);
            return View(teacher);
        }

        _logger.LogWarning($"The teacher with id {id} does not exists.");
        return RedirectToAction("Error", "Home");
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(TeacherModel model)
    {
        _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
        if (ModelState.IsValid)
        {
            _logger.LogInformation($"Checking if the teacher with id {model.Id} exists.");
            if (await _teachersDataProvider.ExistsTeacherAsync(model.Id))
            {
                if (ValidatePersonalId(model.PersonalId))
                {
                    if (model.SelectedDisciplines is null || await CheckDisciplinesExistence(model.SelectedDisciplines))
                    {
                        _logger.LogInformation($"Requesting update teacher.");
                        model.Disciplines ??= new List<DisciplineModel>(model.SelectedDisciplines?.Select(id => new DisciplineModel { Id = id }) ?? new List<DisciplineModel>());
                        bool result = await _teachersDataProvider.UpdateTeacherAsync(model);
                        if (result)
                        {
                            _logger.LogInformation("The teacher was updated successfully.");
                            TempData["teacher-edited"] = true;
                            return RedirectToActionPermanent("Index");
                        }

                        _logger.LogError("Error while updating teacher.");
                        ModelState.AddModelError("Error updating teacher", "Ha ocurrido un error mientras se acutalizaba el profesor.");
                    }
                    else
                    {
                        _logger.LogWarning($"At least one selected discipline does not exist.");
                        ModelState.AddModelError("Error", "Al menos una de las disciplinas seleccionadas no existe.");
                    }
                }
                else
                {
                    _logger.LogWarning($"The personal id {model.PersonalId} is invalid.");
                    ModelState.AddModelError("PersonalId", "El carné de identidad no esta escrito correctamente.");
                }
            }
            else
            {
                _logger.LogWarning($"The teacher with id {model.Id} does not exists.");
                return RedirectToAction("Error", "Home");
            }
        }

        await LoadEditViewModel(model);
        return View(model);
    }

    private async Task LoadEditViewModel(TeacherModel model)
    {
        IList<DisciplineModel> disciplines = await _disciplinesDataProvider.GetDisciplinesAsync();
        model.Disciplines = disciplines;
    }

    [Authorize("Admin")]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
        if (!await _teachersDataProvider.ExistsTeacherAsync(id))
        {
            return BadRequest();
        }
        _logger.LogInformation($"The teacher with id {id} exists.");
        _logger.LogInformation($"Requesting delete the teacher with id {id}.");
        bool result = await _teachersDataProvider.DeleteTeacherAsync(id);
        if (!result)
        {
            return BadRequest();
        }

        _logger.LogInformation($"The teacher with id {id} was eliminated successfully.");
        TempData["teacher-deleted"] = true;
        return Ok();
    }
}