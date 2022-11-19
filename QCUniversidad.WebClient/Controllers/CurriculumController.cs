using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Curriculums;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Auth")]
public class CurriculumsController : Controller
{
    private readonly IDataProvider _dataProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<CurriculumsController> _logger;
    private readonly NavigationSettings _navigationSettings;

    public CurriculumsController(IDataProvider dataProvider, IMapper mapper, IOptions<NavigationSettings> navOptions, ILogger<CurriculumsController> logger)
    {
        _dataProvider = dataProvider;
        _mapper = mapper;
        _logger = logger;
        _navigationSettings = navOptions.Value;
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page = 0)
    {
        _logger.LogRequest(HttpContext);
        try
        {
            _logger.LogInformation($"Loading total curriculums count.");
            var total = await _dataProvider.GetCurriculumsCountAsync();
            _logger.LogInformation("Exists {0} curriculums in total.", total);
            var pageIndex = page - 1 < 0 ? 0 : page - 1;
            var startingItemIndex = pageIndex * _navigationSettings.ItemsPerPage;
            if (startingItemIndex < 0 || startingItemIndex >= total)
            {
                startingItemIndex = 0;
            }
            _logger.LogModelSetLoading<CurriculumsController, CurriculumModel>(HttpContext, startingItemIndex, _navigationSettings.ItemsPerPage);
            var curriculums = await _dataProvider.GetCurriculumsAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
            _logger.LogInformation($"Loaded {curriculums.Count} curriculums.");
            var totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
            var viewModel = new NavigationListViewModel<CurriculumModel>
            {
                Items = curriculums,
                CurrentPage = pageIndex + 1,
                PagesCount = totalPages,
                TotalItems = total
            };
            _logger.LogInformation("Returning view with {0} curriculums, in page {1}, total pages: {2}, total items: {3}", viewModel.ItemsCount, viewModel.CurrentPage, viewModel.PagesCount, viewModel.TotalItems);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Exception throwed {0}", ex.Message);
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid id) => await Task.FromResult(View());

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        _logger.LogRequest(HttpContext);
        var viewmodel = new CreateCurriculumModel();
        await LoadCreateViewModel(viewmodel);
        return View(viewmodel);
    }

    private async Task LoadCreateViewModel(CreateCurriculumModel model)
    {
        await LoadCareersIntoCreateModel(model);
        await LoadDisciplinesIntoCreateModel(model);
    }

    private async Task LoadCareersIntoCreateModel(CreateCurriculumModel model)
    {
        var careers = await _dataProvider.GetCareersAsync();
        model.Careers = careers;
    }

    private async Task LoadDisciplinesIntoCreateModel(CreateCurriculumModel model)
    {
        var disciplines = await _dataProvider.GetDisciplinesAsync();
        model.Disciplines = disciplines;
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(CreateCurriculumModel model)
    {
        _logger.LogRequest(HttpContext);
        if (ModelState.IsValid)
        {
            _logger.LogInformation($"Validating selected disciplines existence.");
            if (model.SelectedDisciplines is null || await CheckDisciplinesExistence(model.SelectedDisciplines))
            {
                _logger.LogCreateModelRequest<CurriculumsController, CurriculumModel>(HttpContext);
                model.Disciplines ??= new List<DisciplineModel>(model.SelectedDisciplines?.Select(id => new DisciplineModel { Id = id }) ?? new List<DisciplineModel>());
                var result = await _dataProvider.CreateCurriculumAsync(model);
                if (result)
                {
                    _logger.LogModelCreated<CurriculumsController, CurriculumModel>(HttpContext);
                    TempData["curriculum-created"] = true;
                    return RedirectToActionPermanent("Index");
                }
                _logger.LogErrorCreatingModel<CurriculumsController, CurriculumModel>(HttpContext);
                ModelState.AddModelError("Error creating curriculum", "Ha ocurrido un error mientras se creaba el curriculum.");
            }
            else
            {
                _logger.LogModelNotExist<CurriculumsController, DisciplineModel>(HttpContext);
                ModelState.AddModelError("Error", "Al menos una de las disciplinas seleccionadas no existe.");
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

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> EditAsync(Guid id)
    {
        _logger.LogRequest(HttpContext);
        _logger.LogCheckModelExistence<CurriculumsController, CurriculumModel>(HttpContext, id);
        if (await _dataProvider.ExistsCurriculumAsync(id))
        {
            var curriculum = await _dataProvider.GetCurriculumAsync(id);
            curriculum.SelectedDisciplines = curriculum.CurriculumDisciplines?.Select(d => d.Id).ToArray();
            curriculum.CurriculumDisciplines?.Clear();
            await LoadEditViewModel(curriculum);
            return View(curriculum);
        }
        _logger.LogModelNotExist<CurriculumsController, CurriculumModel>(HttpContext, id);
        return RedirectToAction("Error", "Home");
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(CurriculumModel model)
    {
        _logger.LogRequest(HttpContext);
        if (ModelState.IsValid)
        {
            _logger.LogCheckModelExistence<CurriculumsController, CurriculumModel>(HttpContext, model.Id);
            if (await _dataProvider.ExistsCurriculumAsync(model.Id))
            {
                if (model.SelectedDisciplines is null || await CheckDisciplinesExistence(model.SelectedDisciplines))
                {
                    _logger.LogEditModelRequest<CurriculumsController, CurriculumModel>(HttpContext);
                    model.CurriculumDisciplines ??= new List<DisciplineModel>(model.SelectedDisciplines?.Select(id => new DisciplineModel { Id = id }) ?? new List<DisciplineModel>());
                    var result = await _dataProvider.UpdateCurriculumAsync(model);
                    if (result)
                    {
                        _logger.LogModelEdited<CurriculumsController, CurriculumModel>(HttpContext);
                        TempData["curriculum-edited"] = true;
                        return RedirectToActionPermanent("Index");
                    }
                    _logger.LogErrorEditingModel<CurriculumsController, CurriculumModel>(HttpContext);
                    ModelState.AddModelError("Error updating curriculum", "Ha ocurrido un error mientras se acutalizaba el curriculum.");
                }
                else
                {
                    _logger.LogModelNotExist<CurriculumsController, DisciplineModel>(HttpContext);
                    ModelState.AddModelError("Error", "Al menos una de las disciplinas seleccionadas no existe.");
                }
            }
            else
            {
                _logger.LogModelNotExist<CurriculumsController, CurriculumModel>(HttpContext);
                return RedirectToAction("Error", "Home");
            }
        }
        await LoadEditViewModel(model);
        return View(model);
    }

    private async Task LoadEditViewModel(CurriculumModel model)
    {
        var disciplines = await _dataProvider.GetDisciplinesAsync();
        model.CurriculumDisciplines = disciplines;
    }

    [Authorize("Admin")]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogRequest(HttpContext);
        if (await _dataProvider.ExistsCurriculumAsync(id))
        {
            _logger.LogDeleteModelRequest<CurriculumsController, CurriculumModel>(HttpContext, id);
            var result = await _dataProvider.DeleteCurriculumAsync(id);
            if (result)
            {
                _logger.LogModelDeleted<CurriculumsController, CurriculumModel>(HttpContext, id);
                TempData["curriculum-deleted"] = true;
                return Ok();
            }
        }
        return BadRequest();
    }
}