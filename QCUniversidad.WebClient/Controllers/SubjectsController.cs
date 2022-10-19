using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Models.Subjects;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;

namespace QCUniversidad.WebClient.Controllers;

[Authorize(Roles = "Administrador")]
public class SubjectsController : Controller
{
    private readonly IDataProvider _dataProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<SubjectsController> _logger;
    private readonly NavigationSettings _navigationSettings;

    public SubjectsController(IDataProvider dataProvider, IMapper mapper, IOptions<NavigationSettings> navOptions, ILogger<SubjectsController> logger)
    {
        _dataProvider = dataProvider;
        _mapper = mapper;
        _logger = logger;
        _navigationSettings = navOptions.Value;
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page = 0)
    {
        _logger.LogRequest(HttpContext);
        try
        {
            _logger.LogInformation($"Loading total subjects count.");
            var total = await _dataProvider.GetSubjectsCountAsync();
            _logger.LogInformation("Exists {0} subjects in total.", total);
            var pageIndex = page - 1 < 0 ? 0 : page - 1;
            var startingItemIndex = pageIndex * _navigationSettings.ItemsPerPage;
            if (startingItemIndex < 0 || startingItemIndex >= total)
            {
                startingItemIndex = 0;
            }
            _logger.LogInformation($"Loading subjects starting in {startingItemIndex} and taking {_navigationSettings.ItemsPerPage}.");
            var subjects = await _dataProvider.GetSubjectsAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
            _logger.LogInformation($"Loaded {subjects.Count} subjects.");
            var totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
            var viewModel = new NavigationListViewModel<SubjectModel>
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

    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        _logger.LogRequest(HttpContext);
        var viewmodel = new CreateSubjectModel();
        await LoadCreateViewModel(viewmodel);
        return View(viewmodel);
    }

    private async Task LoadCreateViewModel(CreateSubjectModel model) => await LoadDisciplinesIntoCreateModel(model);

    private async Task LoadDisciplinesIntoCreateModel(CreateSubjectModel model)
    {
        var disciplines = await _dataProvider.GetDisciplinesAsync();
        model.Disciplines = disciplines;
    }

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
                var model = _mapper.Map<SubjectModel>(createModel);
                var result = await _dataProvider.CreateSubjectAsync(model);
                if (result)
                {
                    _logger.LogModelCreated<SubjectsController, SubjectModel>(HttpContext);
                    TempData["subject-created"] = true;
                    return RedirectToActionPermanent("Index");
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

    [HttpGet]
    public async Task<IActionResult> EditAsync(Guid id)
    {
        _logger.LogRequest(HttpContext);
        _logger.LogCheckModelExistence<SubjectsController, SubjectModel>(HttpContext, id);
        if (await _dataProvider.ExistsSubjectAsync(id))
        {
            var subject = await _dataProvider.GetSubjectAsync(id);
            var model = _mapper.Map<EditSubjectModel>(subject);
            return View(model);
        }
        _logger.LogModelNotExist<SubjectsController, SubjectModel>(HttpContext, id);
        return RedirectToAction("Error", "Home");
    }

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
                var model = _mapper.Map<SubjectModel>(editModel);
                var result = await _dataProvider.UpdateSubjectAsync(model);
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

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogRequest(HttpContext);
        _logger.LogCheckModelExistence<SubjectsController, SubjectModel>(HttpContext, id);
        if (await _dataProvider.ExistsSubjectAsync(id))
        {
            _logger.LogDeleteModelRequest<SubjectsController, SubjectModel>(HttpContext, id);
            var result = await _dataProvider.DeleteSubjectAsync(id);
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