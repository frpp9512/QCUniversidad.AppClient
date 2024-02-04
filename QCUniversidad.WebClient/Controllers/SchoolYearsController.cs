using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.SchoolYears;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Auth")]
public class SchoolYearsController : Controller
{
    private readonly IDataProvider _dataProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<CoursesController> _logger;
    private readonly NavigationSettings _navigationSettings;

    public SchoolYearsController(IDataProvider dataProvider, IMapper mapper, IOptions<NavigationSettings> navOptions, ILogger<CoursesController> logger)
    {
        _dataProvider = dataProvider;
        _mapper = mapper;
        _logger = logger;
        _navigationSettings = navOptions.Value;
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page)
    {
        int total = await _dataProvider.GetFacultiesTotalAsync();
        int pageIndex = page - 1 < 0 ? 0 : page - 1;
        int startingItemIndex = pageIndex * _navigationSettings.ItemsPerPage;
        if (startingItemIndex < 0 || startingItemIndex >= total)
        {
            startingItemIndex = 0;
        }

        IList<SchoolYearModel> schoolYears = await _dataProvider.GetSchoolYearsAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
        int totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
        NavigationListViewModel<SchoolYearModel> viewModel = new()
        {
            Items = schoolYears,
            CurrentPage = pageIndex + 1,
            PagesCount = totalPages,
            TotalItems = total
        };
        return View(viewModel);
    }

    [Authorize("Admin")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(SchoolYearModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _ = await _dataProvider.CreateSchoolYearAsync(model);
                TempData["schoolyear-created"] = true;
                return RedirectToActionPermanent("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Server error", ex.Message);
            }
        }

        return View(model);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid id)
    {
        try
        {
            SchoolYearModel schoolYear = await _dataProvider.GetSchoolYearAsync(id);
            return View(schoolYear);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> EditAsync(Guid id)
    {
        try
        {
            SchoolYearModel schoolYear = await _dataProvider.GetSchoolYearAsync(id);
            return View(schoolYear);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(SchoolYearModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                bool result = await _dataProvider.UpdateSchoolYearAsync(model);
                if (result)
                {
                    TempData["schoolyear-edited"] = true;
                    return RedirectToActionPermanent("Index");
                }

                ModelState.AddModelError("Error de servidor", "Ha ocurrido un problema actualizando el año escolar.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Server error", ex.Message);
            }
        }

        return View(model);
    }

    [Authorize("Admin")]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (await _dataProvider.ExistSchoolYearAsync(id))
            {
                bool result = await _dataProvider.DeleteSchoolYearAsync(id);
                if (result)
                {
                    TempData["schoolyear-deleted"] = true;
                    return Ok($"Se ha eliminado correctamente el año escolar con id {id}.");
                }
                else
                {
                    return Problem($"Ha ocurrido un error eliminando el año escolar con id {id}.");
                }
            }

            return NotFound($"No se ha encontrado el año escolar con id {id}.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
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

            bool result = await _dataProvider.CreatePeriodAsync(_mapper.Map<PeriodModel>(model));
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
        PeriodModel result = await _dataProvider.GetPeriodAsync(id);
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

            bool result = await _dataProvider.UpdatePeriodAsync(model);
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
            bool result = await _dataProvider.DeletePeriodAsync(id);
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