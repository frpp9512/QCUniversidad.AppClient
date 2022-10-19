using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.SchoolYears;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Data;

namespace QCUniversidad.WebClient.Controllers;

[Authorize(Roles = "Administrador")]
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

    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page)
    {
        var total = await _dataProvider.GetFacultiesTotalAsync();
        var pageIndex = page - 1 < 0 ? 0 : page - 1;
        var startingItemIndex = pageIndex * _navigationSettings.ItemsPerPage;
        if (startingItemIndex < 0 || startingItemIndex >= total)
        {
            startingItemIndex = 0;
        }
        var schoolYears = await _dataProvider.GetSchoolYearsAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
        var totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
        NavigationListViewModel<SchoolYearModel> viewModel = new()
        {
            Items = schoolYears,
            CurrentPage = pageIndex + 1,
            PagesCount = totalPages,
            TotalItems = total
        };
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create() => View();

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

    [HttpGet]
    public async Task<IActionResult> EditAsync(Guid id)
    {
        try
        {
            var schoolYear = await _dataProvider.GetSchoolYearAsync(id);
            return View(schoolYear);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(SchoolYearModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = await _dataProvider.UpdateSchoolYearAsync(model);
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

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (await _dataProvider.ExistSchoolYearAsync(id))
            {
                var result = await _dataProvider.DeleteSchoolYearAsync(id);
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
}