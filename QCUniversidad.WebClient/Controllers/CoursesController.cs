using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Course;
using QCUniversidad.WebClient.Models.Curriculums;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Auth")]
public class CoursesController : Controller
{
    private readonly IDataProvider _dataProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<CoursesController> _logger;
    private readonly NavigationSettings _navigationSettings;

    public CoursesController(IDataProvider dataProvider, IMapper mapper, IOptions<NavigationSettings> navOptions, ILogger<CoursesController> logger)
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
            _logger.LogInformation($"Loading total school years count.");
            var total = await _dataProvider.GetCoursesCountAsync();
            _logger.LogInformation("Exists {0} school years in total.", total);
            var pageIndex = page - 1 < 0 ? 0 : page - 1;
            var startingItemIndex = pageIndex * _navigationSettings.ItemsPerPage;
            if (startingItemIndex < 0 || startingItemIndex >= total)
            {
                startingItemIndex = 0;
            }

            _logger.LogModelSetLoading<CoursesController, CourseModel>(HttpContext, startingItemIndex, _navigationSettings.ItemsPerPage);
            var courses = await _dataProvider.GetCoursesAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
            _logger.LogInformation($"Loaded {courses.Count} school years.");
            var totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
            var viewModel = new NavigationListViewModel<CourseModel>
            {
                Items = courses,
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
        _logger.LogCheckModelExistence<CoursesController, CourseModel>(HttpContext, id);
        if (await _dataProvider.ExistsCourseAsync(id))
        {
            _logger.LogModelLoading<CoursesController, CourseModel>(HttpContext, id);
            var model = await _dataProvider.GetCourseAsync(id);
            return View(model);
        }

        return RedirectToAction("Error", "Home");
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        _logger.LogRequest(HttpContext);
        var viewmodel = new CreateCourseModel();
        await LoadCreateViewModel(viewmodel);
        return View(viewmodel);
    }

    private async Task LoadCreateViewModel(CreateCourseModel viewmodel)
    {
        var schoolYears = await _dataProvider.GetSchoolYearsAsync();
        viewmodel.SchoolYears = schoolYears;
        var curriculums = await _dataProvider.GetCurriculumsAsync();
        viewmodel.Curricula = curriculums;
        var careers = await _dataProvider.GetCareersAsync();
        viewmodel.Careers = careers;
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(CreateCourseModel model)
    {
        _logger.LogRequest(HttpContext);
        if (ModelState.IsValid)
        {
            if (await _dataProvider.CheckCourseExistenceByCareerYearAndModality(model.CareerId, model.CareerYear, (int)model.TeachingModality))
            {
                ModelState.AddModelError("Error", "Ya existe un curso que con la carrera, modalidad y año seleccionado.");
            }
            else
            {
                try
                {
                    _logger.LogCreateModelRequest<CoursesController, CourseModel>(HttpContext);
                    var result = await _dataProvider.CreateCourseAsync(model);
                    _logger.LogModelCreated<CoursesController, CourseModel>(HttpContext);
                    TempData["course-created"] = true;
                    return RedirectToAction("Details", new { id = result });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", $"Error agregando el curso. Mensaje: {ex.Message}");
                }
            }
        }

        await LoadCreateViewModel(model);
        return View(model);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> EditAsync(Guid id)
    {
        _logger.LogRequest(HttpContext);
        _logger.LogCheckModelExistence<CoursesController, CourseModel>(HttpContext, id);
        if (await _dataProvider.ExistsCourseAsync(id))
        {
            var course = await _dataProvider.GetCourseAsync(id);
            var editModel = _mapper.Map<EditCourseModel>(course);
            await LoadEditViewModel(editModel);
            return View(editModel);
        }

        _logger.LogModelNotExist<CoursesController, CourseModel>(HttpContext, id);
        return RedirectToAction("Error", "Home");
    }

    private async Task LoadEditViewModel(EditCourseModel viewmodel)
    {
        var curriculums = await _dataProvider.GetCurriculumsForCareerAsync(viewmodel.CareerId);
        viewmodel.Curricula = curriculums;
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> GetCurriculumOptionsForCareerAsync(Guid careerId)
    {
        try
        {
            var curriculums = await _dataProvider.GetCurriculumsForCareerAsync(careerId);
            return PartialView("_CurriculumOptions", curriculums);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(EditCourseModel model)
    {
        _logger.LogRequest(HttpContext);
        if (ModelState.IsValid)
        {
            _logger.LogCheckModelExistence<CoursesController, CourseModel>(HttpContext, model.Id);
            if (!await _dataProvider.ExistsCourseAsync(model.Id))
            {
                ModelState.AddModelError("Error", "El curso no existe.");
            }
            else
            {
                _logger.LogCheckModelExistence<CoursesController, CareerModel>(HttpContext, model.CareerId);
                if (!await _dataProvider.ExistsCareerAsync(model.CareerId))
                {
                    ModelState.AddModelError("Error", "La carrera no existe.");
                }
                else
                {
                    _logger.LogCheckModelExistence<CoursesController, CurriculumModel>(HttpContext, model.CurriculumId);
                    if (!await _dataProvider.ExistsCurriculumAsync(model.CurriculumId))
                    {
                        ModelState.AddModelError("Error", "El curriculum no existe.");
                    }
                    else
                    {
                        _logger.LogEditModelRequest<CoursesController, CourseModel>(HttpContext, model.Id);
                        var result = await _dataProvider.UpdateCourseAsync(model);
                        if (result)
                        {
                            _logger.LogModelEdited<CoursesController, CourseModel>(HttpContext, model.Id);
                            TempData["course-edited"] = true;
                            return RedirectToAction("Index");
                        }

                        ModelState.AddModelError("Error", "Error actualizando el curso");
                    }
                }
            }
        }

        await LoadEditViewModel(model);
        return View(model);
    }

    [Authorize("Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        _logger.LogRequest(HttpContext);
        _logger.LogCheckModelExistence<CoursesController, CourseModel>(HttpContext, id);
        if (await _dataProvider.ExistsCourseAsync(id))
        {
            _logger.LogDeleteModelRequest<CoursesController, CourseModel>(HttpContext, id);
            var result = await _dataProvider.DeleteCourseAsync(id);
            if (result)
            {
                _logger.LogModelDeleted<CoursesController, CourseModel>(HttpContext, id);
                TempData["course-deleted"] = true;
                return Ok(result);
            }
        }

        return NotFound(id);
    }

    [Authorize("Admin")]
    [HttpPut]
    public async Task<IActionResult> CreatePeriodAsync([FromBody] CreatePeriodModel model)
    {
        _logger.LogRequest(HttpContext);
        try
        {
            if (!await _dataProvider.ExistSchoolYearAsync(model.SchoolYearId))
            {
                return NotFound("El año escolar no existe.");
            }

            if (model.Starts >= model.Ends)
            {
                return BadRequest(new { responseText = "La fecha de culminación debe de suceder a la de inicio." });
            }

            var result = await _dataProvider.CreatePeriodAsync(_mapper.Map<PeriodModel>(model));
            if (result)
            {
                TempData["period-created"] = true;
                return Ok(new { responseText = result });
            }

            return Problem("Ha ocurrido un problema creando el período.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> GetPeriodAsync(Guid id)
    {
        var result = await _dataProvider.GetPeriodAsync(id);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Authorize("Admin")]
    [HttpPost]
    public async Task<IActionResult> UpdatePeriodAsync(PeriodModel model)
    {
        _logger.LogRequest(HttpContext);
        try
        {
            if (!await _dataProvider.ExistsPeriodAsync(model.Id))
            {
                return NotFound(new { responseText = "El periodo no existe." });
            }

            if (!await _dataProvider.ExistSchoolYearAsync(model.SchoolYearId))
            {
                return NotFound(new { responseText = "El año escolar no existe." });
            }

            if (model.Starts >= model.Ends)
            {
                return BadRequest(new { responseText = "La fecha de culminación debe de suceder a la de inicio." });
            }

            var result = await _dataProvider.UpdatePeriodAsync(model);
            if (result)
            {
                TempData["period-updated"] = true;
                return Ok(new { responseText = result });
            }

            return Problem("Ha ocurrido un problema actualizando el período.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [Authorize("Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeletePeriodAsync(Guid id)
    {
        _logger.LogRequest(HttpContext);
        if (await _dataProvider.ExistsPeriodAsync(id))
        {
            var result = await _dataProvider.DeletePeriodAsync(id);
            if (result)
            {
                TempData["period-deleted"] = true;
                return Ok(new { responseText = "ok" });
            }

            return Problem("Ha ocurrido un problema eliminando el período.");
        }

        return NotFound(new { responseText = $"No existe el período con id {id}" });
    }
}