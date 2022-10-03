using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Curriculums;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.SchoolYears;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Controllers;

[Authorize]
public class SchoolYearsController : Controller
{
    private readonly IDataProvider _dataProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<SchoolYearsController> _logger;
    private readonly NavigationSettings _navigationSettings;

    public SchoolYearsController(IDataProvider dataProvider, IMapper mapper, IOptions<NavigationSettings> navOptions, ILogger<SchoolYearsController> logger)
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
            _logger.LogInformation($"Loading total school years count.");
            var total = await _dataProvider.GetSchoolYearsCountAsync();
            _logger.LogInformation("Exists {0} school years in total.", total);
            var pageIndex = page - 1 < 0 ? 0 : page - 1;
            var startingItemIndex = (pageIndex * _navigationSettings.ItemsPerPage);
            if (startingItemIndex < 0 || startingItemIndex >= total)
            {
                startingItemIndex = 0;
            }
            _logger.LogModelSetLoading<SchoolYearsController, SchoolYearModel>(HttpContext, startingItemIndex, _navigationSettings.ItemsPerPage);
            var schoolYears = await _dataProvider.GetSchoolYearsAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
            _logger.LogInformation($"Loaded {schoolYears.Count} school years.");
            var totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
            var viewModel = new NavigationListViewModel<SchoolYearModel>
            {
                Items = schoolYears,
                CurrentPage = pageIndex + 1,
                PagesCount = totalPages,
                TotalItems = total
            };
            _logger.LogInformation("Returning view with {0} school years, in page {1}, total pages: {2}, total items: {3}", viewModel.ItemsCount, viewModel.CurrentPage, viewModel.PagesCount, viewModel.TotalItems);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Exception throwed {0}", ex.Message);
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid id)
    {
        _logger.LogRequest(HttpContext);
        _logger.LogCheckModelExistence<SchoolYearsController, SchoolYearModel>(HttpContext, id);
        if (await _dataProvider.ExistsSchoolYearAsync(id))
        {
            _logger.LogModelLoading<SchoolYearsController, SchoolYearModel>(HttpContext, id);
            var model = await _dataProvider.GetSchoolYearAsync(id);
            return View(model);
        }
        return RedirectToAction("Error", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        _logger.LogRequest(HttpContext);
        var viewmodel = new CreateSchoolYearModel();
        await LoadCreateViewModel(viewmodel);
        return View(viewmodel);
    }

    private async Task LoadCreateViewModel(CreateSchoolYearModel viewmodel)
    {
        var curriculums = await _dataProvider.GetCurriculumsAsync();
        viewmodel.Curricula = curriculums;
        var careers = await _dataProvider.GetCareersAsync();
        viewmodel.Careers = careers;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(CreateSchoolYearModel model)
    {
        _logger.LogRequest(HttpContext);
        if (ModelState.IsValid)
        {
            if (model.Starts >= model.Ends)
            {
                ModelState.AddModelError("Starts", "La fecha de inicio del curso no debe de ser antes de la de culminación");
            }
            else
            {
                if (await _dataProvider.CheckSchoolYearExistenceByCareerYearAndModality(model.CareerId, model.CareerYear, (int)model.TeachingModality))
                {
                    ModelState.AddModelError("Error", "Ya existe un año escolar que con la carrera, modalidad y año seleccionado.");
                }
                else
                {
                    try
                    {
                        _logger.LogCreateModelRequest<SchoolYearsController, SchoolYearModel>(HttpContext);
                        var result = await _dataProvider.CreateSchoolYearAsync(model);
                        _logger.LogModelCreated<SchoolYearsController, SchoolYearModel>(HttpContext);
                        TempData["schoolyear-created"] = true;
                        return RedirectToAction("Details", new { id = result });
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Error", $"Error agregando el año escolar. Mensaje: { ex.Message }");
                    }
                }
            }
        }
        await LoadCreateViewModel(model);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditAsync(Guid id)
    {
        _logger.LogRequest(HttpContext);
        _logger.LogCheckModelExistence<SchoolYearsController, SchoolYearModel>(HttpContext, id);
        if (await _dataProvider.ExistsSchoolYearAsync(id))
        {
            var schoolYear = await _dataProvider.GetSchoolYearAsync(id);
            var editModel = _mapper.Map<EditSchoolYearModel>(schoolYear);
            await LoadEditViewModel(editModel);
            return View(editModel);
        }
        _logger.LogModelNotExist<SchoolYearsController, SchoolYearModel>(HttpContext, id);
        return RedirectToAction("Error", "Home");
    }

    private async Task LoadEditViewModel(EditSchoolYearModel viewmodel)
    {
        var curriculums = await _dataProvider.GetCurriculumsAsync();
        viewmodel.Curricula = curriculums;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(EditSchoolYearModel model)
    {
        _logger.LogRequest(HttpContext);
        if (ModelState.IsValid)
        {
            _logger.LogCheckModelExistence<SchoolYearsController, SchoolYearModel>(HttpContext, model.Id);
            if (!await _dataProvider.ExistsSchoolYearAsync(model.Id))
            {
                ModelState.AddModelError("Error", "El año escolar no existe.");
            }
            else
            {
                _logger.LogCheckModelExistence<SchoolYearsController, CareerModel>(HttpContext, model.CareerId);
                if (!await _dataProvider.ExistsCareerAsync(model.CareerId))
                {
                    ModelState.AddModelError("Error", "La carrera no existe.");
                }
                else
                {
                    _logger.LogCheckModelExistence<SchoolYearsController, CurriculumModel>(HttpContext, model.CurriculumId);
                    if (!await _dataProvider.ExistsCurriculumAsync(model.CurriculumId))
                    {
                        ModelState.AddModelError("Error", "El curriculum no existe.");
                    }
                    else
                    {
                        if (model.Starts >= model.Ends)
                        {
                            ModelState.AddModelError("Starts", "La fecha de inicio del curso no debe de ser antes de la de culminación");
                        }
                        else
                        {
                            if (await _dataProvider.CheckSchoolYearExistenceByCareerYearAndModality(model.CareerId, model.CareerYear, (int)model.TeachingModality))
                            {
                                ModelState.AddModelError("Error", "Ya existe un año escolar con la carrera, modalidad y año seleccionado.");
                            }
                            else
                            {
                                _logger.LogEditModelRequest<SchoolYearsController, SchoolYearModel>(HttpContext, model.Id);
                                var result = await _dataProvider.UpdateSchoolYearAsync(model);
                                if (result)
                                {
                                    _logger.LogModelEdited<SchoolYearsController, SchoolYearModel>(HttpContext, model.Id);
                                    TempData["schoolyear-edited"] = true;
                                    return RedirectToAction("Index");
                                }
                                ModelState.AddModelError("Error", "Error actualizando el año escolar");
                            }
                        }
                    }
                }
            }
        }
        await LoadEditViewModel(model);
        return View(model);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        _logger.LogRequest(HttpContext);
        _logger.LogCheckModelExistence<SchoolYearsController, SchoolYearModel>(HttpContext, id);
        if (await _dataProvider.ExistsSchoolYearAsync(id))
        {
            _logger.LogDeleteModelRequest<SchoolYearsController, SchoolYearModel>(HttpContext, id);
            var result = await _dataProvider.DeleteSchoolYearAsync(id);
            if (result)
            {
                _logger.LogModelDeleted<SchoolYearsController, SchoolYearModel>(HttpContext, id);
                TempData["schoolyear-deleted"] = true;
                return Ok(result);
            }
        }
        return NotFound(id);
    }

    [HttpPut]
    public async Task<IActionResult> CreatePeriodAsync([FromBody] CreatePeriodModel model)
    {
        _logger.LogRequest(HttpContext);
        try
        {
            if (!await _dataProvider.ExistsSchoolYearAsync(model.SchoolYearId))
            {
                return NotFound("El año escolar no existe.");
            }
            if (model.Starts >= model.Ends)
            {
                return BadRequest(new { responseText = "La fecha de culminación debe de suceder a la de inicio." });
            }
            var result = await _dataProvider.CreatePeriodAsync(_mapper.Map<PeriodModel>(model));
            return result ? Ok(result) : (IActionResult)Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}