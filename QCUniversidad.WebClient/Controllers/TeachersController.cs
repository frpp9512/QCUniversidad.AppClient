using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Models.Teachers;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Extensions;

namespace QCUniversidad.WebClient.Controllers;

[Authorize(Roles = "Administrador")]
public class TeachersController : Controller
{
    private readonly IDataProvider _dataProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<TeachersController> _logger;
    private readonly IExcelParser<TeacherModel> _teachersExcelParser;
    private readonly NavigationSettings _navigationSettings;

    public TeachersController(IDataProvider dataProvider, IMapper mapper, IOptions<NavigationSettings> navOptions, ILogger<TeachersController> logger, IExcelParser<TeacherModel> teachersExcelParser)
    {
        _dataProvider = dataProvider;
        _mapper = mapper;
        _logger = logger;
        _teachersExcelParser = teachersExcelParser;
        _navigationSettings = navOptions.Value;
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page = 0)
    {
        _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
        try
        {
            _logger.LogInformation($"Loading total teachers count.");
            var total = await _dataProvider.GetTeachersCountAsync();
            _logger.LogInformation("Exists {0} teachers in total.", total);
            var pageIndex = page - 1 < 0 ? 0 : page - 1;
            var startingItemIndex = pageIndex * _navigationSettings.ItemsPerPage;
            if (startingItemIndex < 0 || startingItemIndex >= total)
            {
                startingItemIndex = 0;
            }
            _logger.LogInformation($"Loading teachers starting in {startingItemIndex} and taking {_navigationSettings.ItemsPerPage}.");
            var teachers = await _dataProvider.GetTeachersAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
            _logger.LogInformation($"Loaded {teachers.Count} teachers.");
            var totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
            var viewModel = new NavigationListViewModel<TeacherModel>
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

    [HttpGet]
    public async Task<IActionResult> ImportAsync()
    {
        var departments = await _dataProvider.GetDepartmentsAsync();
        ViewData["departments-list"] = departments;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportAsync(IFormFile formFile, Guid selectedDepartment)
    {
        var fileStream = formFile.OpenReadStream();
        var parsedModels = await GetParsedModelsAsync(fileStream);
        if (!parsedModels.Any())
        {
            TempData["importing-error"] = "No se ha podido importar ningún profesor.";
        }
        var created = 0;
        var updated = 0;
        var failed = 0;
        foreach (var newTeacher in parsedModels.Where(t => t.ImportAction == TeacherImportAction.Create))
        {
            try
            {
                newTeacher.DepartmentId = selectedDepartment;
                await _dataProvider.CreateTeacherAsync(newTeacher);
                created++;
            }
            catch
            {
                failed++;
            }
        }
        foreach (var teacherToUpdate in parsedModels.Where(t => t.ImportAction == TeacherImportAction.Update))
        {
            try
            {
                var teacher = await _dataProvider.GetTeacherAsync(teacherToUpdate.PersonalId);
                teacher.DepartmentId = selectedDepartment;
                teacher.Fullname = teacherToUpdate.Fullname;
                teacher.Position = teacherToUpdate.Position;
                teacher.Category = teacherToUpdate.Category;
                teacher.Email = teacherToUpdate.Email;
                teacher.ContractType = teacherToUpdate.ContractType;
                await _dataProvider.UpdateTeacherAsync(teacher);
            }
            catch
            {
                failed++;
            }
        }
        TempData["importing-result"] = $"Se han importado un total de {(created + updated)} profesores, creando {created} y actualizando {updated}, con {failed} fallos.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ImportFilePreviewAsync(IFormFile formFile)
    {
        var fileStream = formFile.OpenReadStream();
        var parsedModels = await GetParsedModelsAsync(fileStream);
        return Json(parsedModels);
    }

    private async Task<IList<TeacherModel>> GetParsedModelsAsync(Stream fileStream)
    {
        var parsedModels = await _teachersExcelParser.ParseExcelAsync(fileStream);
        foreach (var parsedModel in parsedModels)
        {
            TeacherImportAction action;
            if (!ValidatePersonalId(parsedModel.PersonalId))
            {
                action = TeacherImportAction.NoImport;
            }
            else
            {
                var exists = await _dataProvider.ExistsTeacherAsync(parsedModel.PersonalId);
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
        var templateBytes = await System.IO.File.ReadAllBytesAsync("templates/teachers_import.xlsx");
        return File(templateBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QCU Plantilla para importar profesores.xlsx");
    }

    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
        var viewmodel = new CreateTeacherModel();
        await LoadCreateViewModel(viewmodel);
        return View(viewmodel);
    }

    private async Task LoadCreateViewModel(CreateTeacherModel model)
    {
        await LoadDisciplinesIntoCreateModel(model);
        var departments = await _dataProvider.GetDepartmentsAsync();
        model.DepartmentList = departments;
    }

    private async Task LoadDisciplinesIntoCreateModel(CreateTeacherModel model)
    {
        var disciplines = await _dataProvider.GetDisciplinesAsync();
        model.Disciplines = disciplines;
    }

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
                if (await _dataProvider.ExistsDepartmentAsync(model.DepartmentId))
                {
                    _logger.LogInformation($"Validating selected disciplines existence.");
                    if (model.SelectedDisciplines is null || await CheckDisciplinesExistence(model.SelectedDisciplines))
                    {
                        _logger.LogInformation($"Requesting create new teacher.");
                        model.Disciplines ??= new List<DisciplineModel>(model.SelectedDisciplines?.Select(id => new DisciplineModel { Id = id }) ?? new List<DisciplineModel>());
                        var result = await _dataProvider.CreateTeacherAsync(model);
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
        foreach (var id in disciplinesIds)
        {
            if (!await _dataProvider.ExistsDisciplineAsync(id))
            {
                return false;
            }
        }
        return true;
    }

    private bool ValidatePersonalId(string personalId) => (personalId.Length == 11)
            && personalId.Any(c => char.IsNumber(c))
            && (int.Parse(personalId.Substring(2, 2)) <= 12)
            && (int.Parse(personalId.Substring(4, 2)) <= 31);

    [HttpGet]
    public async Task<IActionResult> EditAsync(Guid id)
    {
        _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
        _logger.LogInformation($"Checking if the teacher with id {id} exists.");
        if (await _dataProvider.ExistsTeacherAsync(id))
        {
            var teacher = await _dataProvider.GetTeacherAsync(id);
            teacher.SelectedDisciplines = teacher.Disciplines?.Select(d => d.Id).ToArray();
            teacher.Disciplines?.Clear();
            await LoadEditViewModel(teacher);
            return View(teacher);
        }
        _logger.LogWarning($"The teacher with id {id} does not exists.");
        return RedirectToAction("Error", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(TeacherModel model)
    {
        _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
        if (ModelState.IsValid)
        {
            _logger.LogInformation($"Checking if the teacher with id {model.Id} exists.");
            if (await _dataProvider.ExistsTeacherAsync(model.Id))
            {
                if (ValidatePersonalId(model.PersonalId))
                {
                    if (model.SelectedDisciplines is null || await CheckDisciplinesExistence(model.SelectedDisciplines))
                    {
                        _logger.LogInformation($"Requesting update teacher.");
                        model.Disciplines ??= new List<DisciplineModel>(model.SelectedDisciplines?.Select(id => new DisciplineModel { Id = id }) ?? new List<DisciplineModel>());
                        var result = await _dataProvider.UpdateTeacherAsync(model);
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
        var disciplines = await _dataProvider.GetDisciplinesAsync();
        model.Disciplines = disciplines;
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
        if (await _dataProvider.ExistsTeacherAsync(id))
        {
            _logger.LogInformation($"The teacher with id {id} exists.");
            _logger.LogInformation($"Requesting delete the teacher with id {id}.");
            var result = await _dataProvider.DeleteTeacherAsync(id);
            if (result)
            {
                _logger.LogInformation($"The teacher with id {id} was eliminated successfully.");
                TempData["teacher-deleted"] = true;
                return Ok();
            }
        }
        return BadRequest();
    }
}