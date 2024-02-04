using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Models.Subjects;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Extensions;
using QCUniversidad.WebClient.Services.Platform;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Auth")]
public class SubjectsController : Controller
{
    private readonly IDataProvider _dataProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<SubjectsController> _logger;
    private readonly IExcelParser<SubjectModel> _excelParser;
    private readonly NavigationSettings _navigationSettings;

    public SubjectsController(IDataProvider dataProvider,
                              IMapper mapper,
                              IOptions<NavigationSettings> navOptions,
                              ILogger<SubjectsController> logger,
                              IExcelParser<SubjectModel> excelParser)
    {
        _dataProvider = dataProvider;
        _mapper = mapper;
        _logger = logger;
        _excelParser = excelParser;
        _navigationSettings = navOptions.Value;
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page = 0)
    {
        _logger.LogRequest(HttpContext);
        try
        {
            _logger.LogInformation($"Loading total subjects count.");
            int total = await _dataProvider.GetSubjectsCountAsync();
            _logger.LogInformation("Exists {0} subjects in total.", total);
            int pageIndex = page - 1 < 0 ? 0 : page - 1;
            int startingItemIndex = pageIndex * _navigationSettings.ItemsPerPage;
            if (startingItemIndex < 0 || startingItemIndex >= total)
            {
                startingItemIndex = 0;
            }

            _logger.LogInformation($"Loading subjects starting in {startingItemIndex} and taking {_navigationSettings.ItemsPerPage}.");
            IList<SubjectModel> subjects = await _dataProvider.GetSubjectsAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
            _logger.LogInformation($"Loaded {subjects.Count} subjects.");
            int totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
            NavigationListViewModel<SubjectModel> viewModel = new()
            {
                Items = subjects,
                CurrentPage = pageIndex + 1,
                PagesCount = totalPages,
                TotalItems = total
            };
            _logger.LogInformation("Returning view with {0} subjects, in page {1}, total pages: {2}, total items: {3}", viewModel.ItemsCount, viewModel.CurrentPage, viewModel.PagesCount, viewModel.TotalItems);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Exception throwed {0}", ex.Message);
            return RedirectToActionPermanent("Error", "Home");
        }
    }

    [Authorize("Auth")]
    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return RedirectToAction("Error", "Home");
        }

        try
        {
            SubjectModel subject = await _dataProvider.GetSubjectAsync(id);
            Models.SchoolYears.SchoolYearModel schoolYear = await _dataProvider.GetCurrentSchoolYear();
            ViewData["schoolYear"] = schoolYear;
            return View(subject);
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
        IList<DisciplineModel> disciplines = await _dataProvider.GetDisciplinesAsync();
        ViewData["disciplines-list"] = disciplines;
        return View();
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportAsync(IFormFile formFile, Guid selectedDiscipline)
    {
        Stream fileStream = formFile.OpenReadStream();
        IList<SubjectModel> parsedModels = await GetParsedModelsAsync(fileStream);
        if (!parsedModels.Any())
        {
            TempData["importing-error"] = "No se ha podido importar ninguna asignatura.";
        }

        int created = 0;
        int updated = 0;
        int failed = 0;
        foreach (SubjectModel? newSubject in parsedModels.Where(s => s.ImportAction == SubjectImportAction.Create))
        {
            try
            {
                newSubject.DisciplineId = selectedDiscipline;
                _ = await _dataProvider.CreateSubjectAsync(newSubject);
                created++;
            }
            catch
            {
                failed++;
            }
        }

        foreach (SubjectModel? subjectToUpdate in parsedModels.Where(t => t.ImportAction == SubjectImportAction.Update))
        {
            try
            {
                SubjectModel subject = await _dataProvider.GetSubjectAsync(subjectToUpdate.Name);
                subject.Description = subjectToUpdate.Description;
                _ = await _dataProvider.UpdateSubjectAsync(subject);
            }
            catch
            {
                failed++;
            }
        }

        TempData["importing-result"] = $"Se han importado un total de {created + updated} asignaturas, creando {created} y actualizando {updated}, con {failed} fallos.";
        return RedirectToAction("Index");
    }

    [Authorize("Admin")]
    [HttpPost]
    public async Task<IActionResult> ImportFilePreviewAsync(IFormFile formFile)
    {
        Stream fileStream = formFile.OpenReadStream();
        IList<SubjectModel> parsedModels = await GetParsedModelsAsync(fileStream);
        return Json(parsedModels);
    }

    private async Task<IList<SubjectModel>> GetParsedModelsAsync(Stream fileStream)
    {
        IList<SubjectModel> parsedModels = await _excelParser.ParseExcelAsync(fileStream);
        foreach (SubjectModel parsedModel in parsedModels)
        {
            bool exists = await _dataProvider.ExistsSubjectAsync(parsedModel.Name);
            SubjectImportAction action = exists ? SubjectImportAction.Update : SubjectImportAction.Create;
            parsedModel.ImportAction = action;
        }

        return parsedModels;
    }

    public async Task<IActionResult> TemplateFileAsync()
    {
        if (!System.IO.File.Exists("templates/disciplines_import.xlsx"))
        {
            return NotFound("The template file is missing!");
        }

        byte[] templateBytes = await System.IO.File.ReadAllBytesAsync("templates/subjects_import.xlsx");
        return File(templateBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QCU Plantilla para importar asignaturas.xlsx");
    }

    [Authorize("Planner")]
    [HttpGet]
    public async Task<IActionResult> CreateAsync(string returnTo = "Index")
    {
        _logger.LogRequest(HttpContext);
        CreateSubjectModel viewmodel = new() { Name = "" };
        await LoadCreateViewModel(viewmodel);
        viewmodel.ReturnTo = returnTo;
        return View(viewmodel);
    }

    private async Task LoadCreateViewModel(CreateSubjectModel model)
    {
        await LoadDisciplinesIntoCreateModel(model);
    }

    private async Task LoadDisciplinesIntoCreateModel(CreateSubjectModel model)
    {
        IList<DisciplineModel> disciplines = await _dataProvider.GetDisciplinesAsync();
        model.Disciplines = disciplines;
    }

    [Authorize("Planner")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(CreateSubjectModel createModel)
    {
        _logger.LogRequest(HttpContext);
        if (ModelState.IsValid)
        {
            _logger.LogCheckModelExistence<SubjectsController, DisciplineModel>(HttpContext, createModel.DisciplineId);
            if (await _dataProvider.ExistsDisciplineAsync(createModel.DisciplineId))
            {
                _logger.LogCreateModelRequest<SubjectsController, SubjectModel>(HttpContext);
                SubjectModel model = _mapper.Map<SubjectModel>(createModel);
                bool result = await _dataProvider.CreateSubjectAsync(model);
                if (result)
                {
                    _logger.LogModelCreated<SubjectsController, SubjectModel>(HttpContext);
                    TempData["subject-created"] = true;
                    return Redirect(createModel.ReturnTo ?? "Index");
                }

                _logger.LogErrorCreatingModel<SubjectsController, SubjectModel>(HttpContext, result);
                ModelState.AddModelError("Error creating subject", "Ha ocurrido un error mientras se creaba la asignatura.");
            }
            else
            {
                _logger.LogModelNotExist<SubjectsController, DisciplineModel>(HttpContext, createModel.DisciplineId);
                ModelState.AddModelError("Error disciplina", "La disciplina no existe.");
            }
        }

        await LoadCreateViewModel(createModel);
        return View(createModel);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> EditAsync(Guid id)
    {
        _logger.LogRequest(HttpContext);
        _logger.LogCheckModelExistence<SubjectsController, SubjectModel>(HttpContext, id);
        if (await _dataProvider.ExistsSubjectAsync(id))
        {
            SubjectModel subject = await _dataProvider.GetSubjectAsync(id);
            EditSubjectModel model = _mapper.Map<EditSubjectModel>(subject);
            return View(model);
        }

        _logger.LogModelNotExist<SubjectsController, SubjectModel>(HttpContext, id);
        return RedirectToAction("Error", "Home");
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(EditSubjectModel editModel)
    {
        _logger.LogRequest(HttpContext);
        if (ModelState.IsValid)
        {
            _logger.LogCheckModelExistence<SubjectsController, SubjectModel>(HttpContext, editModel.Id);
            if (await _dataProvider.ExistsSubjectAsync(editModel.Id))
            {
                _logger.LogEditModelRequest<SubjectsController, SubjectModel>(HttpContext, editModel.Id);
                SubjectModel model = _mapper.Map<SubjectModel>(editModel);
                bool result = await _dataProvider.UpdateSubjectAsync(model);
                if (result)
                {
                    _logger.LogModelEdited<SubjectsController, SubjectModel>(HttpContext, editModel.Id);
                    _logger.LogInformation("The subject was updated successfully.");
                    TempData["subject-edited"] = true;
                    return RedirectToActionPermanent("Index");
                }

                _logger.LogErrorEditingModel<SubjectsController, SubjectModel>(HttpContext, editModel.Id);
                ModelState.AddModelError("Error updating subect", "Ha ocurrido un error mientras se actualizaba la asignatura.");
            }
            else
            {
                _logger.LogModelNotExist<SubjectsController, SubjectModel>(HttpContext, editModel.Id);
                return RedirectToAction("Error", "Home");
            }
        }

        return View(editModel);
    }

    [Authorize("Admin")]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogRequest(HttpContext);
        _logger.LogCheckModelExistence<SubjectsController, SubjectModel>(HttpContext, id);
        if (await _dataProvider.ExistsSubjectAsync(id))
        {
            _logger.LogDeleteModelRequest<SubjectsController, SubjectModel>(HttpContext, id);
            bool result = await _dataProvider.DeleteSubjectAsync(id);
            if (result)
            {
                _logger.LogModelDeleted<SubjectsController, SubjectModel>(HttpContext, id);
                TempData["subject-deleted"] = true;
                return Ok();
            }
        }

        _logger.LogModelNotExist<SubjectsController, SubjectModel>(HttpContext, id);
        return BadRequest();
    }
}