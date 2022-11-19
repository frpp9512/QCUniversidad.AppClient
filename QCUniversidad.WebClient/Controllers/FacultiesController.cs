﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Data;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Auth")]
public class FacultiesController : Controller
{
    private readonly IDataProvider _dataProvider;
    private readonly NavigationSettings _navigationSettings;

    public FacultiesController(IDataProvider dataProvider, IOptions<NavigationSettings> navSettingsOptions)
    {
        _dataProvider = dataProvider;
        _navigationSettings = navSettingsOptions.Value;
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page = 1)
    {
        var total = await _dataProvider.GetFacultiesTotalAsync();
        var pageIndex = page - 1 < 0 ? 0 : page - 1;
        var startingItemIndex = pageIndex * _navigationSettings.ItemsPerPage;
        if (startingItemIndex < 0 || startingItemIndex >= total)
        {
            startingItemIndex = 0;
        }
        var faculties = await _dataProvider.GetFacultiesAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
        var totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
        var viewModel = new NavigationListViewModel<FacultyModel>
        {
            Items = faculties,
            CurrentPage = pageIndex + 1,
            PagesCount = totalPages,
            TotalItems = total
        };
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return RedirectToAction("Error", "Home");
        }

        var faculty = await _dataProvider.GetFacultyAsync(id);
        var schoolYear = await _dataProvider.GetCurrentSchoolYear();
        ViewData["schoolYear"] = schoolYear;

        return View(faculty);
    }

    [HttpGet]
    public async Task<IActionResult> FacultyCareersAsync(Guid facultyId)
    {
        if (facultyId == Guid.Empty)
        {
            return BadRequest();
        }
        try
        {
            var careers = await _dataProvider.GetCareersAsync(facultyId);
            return PartialView("_FacultyCareers", careers);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> FacultyDepartmentsAsync(Guid facultyId)
    {
        if (facultyId == Guid.Empty)
        {
            return BadRequest();
        }
        try
        {
            var departments = await _dataProvider.GetDepartmentsAsync(facultyId);
            return PartialView("_FacultyDepartments", departments);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message);
        }
    }

    [Authorize("Admin")]
    [HttpGet]
    public IActionResult Create() => View();

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(FacultyModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _ = await _dataProvider.CreateFacultyAsync(model);
                TempData["faculty-created"] = true;
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
    public async Task<IActionResult> EditAsync(Guid id)
    {
        try
        {
            var faculty = await _dataProvider.GetFacultyAsync(id);
            return View(faculty);
        }
        catch (Exception)
        {
            return RedirectToActionPermanent("Error", "Home");
        }
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(FacultyModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = await _dataProvider.UpdateFacultyAsync(model);
                if (result)
                {
                    TempData["faculty-edited"] = true;
                    return RedirectToActionPermanent("Index");
                }
                ModelState.AddModelError("Error de servidor", "Ha ocurrido un problema actualizando la facultad.");
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
            if (await _dataProvider.ExistFacultyAsync(id))
            {
                var result = await _dataProvider.DeleteFacultyAsync(id);
                if (result)
                {
                    TempData["faculty-deleted"] = true;
                    return Ok($"Se ha eliminado correctamente la facultad con id {id}.");
                }
                else
                {
                    return Problem($"Ha ocurrido un error eliminando la facultad con id {id}.");
                }
            }
            return NotFound($"No se ha encontrado la facultad con id {id}.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}