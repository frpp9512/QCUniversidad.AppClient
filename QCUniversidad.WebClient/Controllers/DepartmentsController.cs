using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Data;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Auth")]
public class DepartmentsController : Controller
{
    private readonly IDataProvider _dataProvider;
    private readonly IMapper _mapper;
    private readonly NavigationSettings _navigationSettings;

    public DepartmentsController(IDataProvider dataProvider, IOptions<NavigationSettings> navSettingsOptions, IMapper mapper)
    {
        _dataProvider = dataProvider;
        _mapper = mapper;
        _navigationSettings = navSettingsOptions.Value;
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page = 1)
    {
        try
        {
            var total = await _dataProvider.GetDepartmentsCountAsync();
            var pageIndex = page - 1 < 0 ? 0 : page - 1;
            var startingItemIndex = pageIndex * _navigationSettings.ItemsPerPage;
            if (startingItemIndex < 0 || startingItemIndex >= total)
            {
                startingItemIndex = 0;
            }

            var departments = await _dataProvider.GetDepartmentsAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
            var totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
            var viewModel = new NavigationListViewModel<DepartmentModel>
            {
                Items = departments,
                CurrentPage = pageIndex + 1,
                PagesCount = totalPages,
                TotalItems = total
            };
            return View(viewModel);
        }
        catch (Exception)
        {
            return RedirectToActionPermanent("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return RedirectToAction("Error", "Home");
        }

        try
        {
            var department = await _dataProvider.GetDepartmentAsync(id);
            var schoolYear = await _dataProvider.GetCurrentSchoolYear();
            ViewData["schoolYear"] = schoolYear;
            return View(department);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> DepartmentDisciplinesAsync(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest();
        }

        try
        {
            var disciplines = await _dataProvider.GetDisciplinesAsync(departmentId);
            return PartialView("_DepartmentDisciplines", disciplines);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> DepartmentTeachersAsync(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest();
        }

        try
        {
            var teachers = await _dataProvider.GetTeachersOfDepartmentAsync(departmentId);
            return PartialView("_DepartmentTeachers", teachers);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> DepartmentCareersAsync(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest();
        }

        try
        {
            var careers = await _dataProvider.GetCareersForDepartmentAsync(departmentId);
            return PartialView("_DepartmentCareers", careers);
        }
        catch (Exception ex) { return Problem(detail: ex.Message); }
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> LoadViewAsync()
    {
        var schoolYears = await _dataProvider.GetSchoolYearsAsync();
        return View(schoolYears);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> GetPeriodOptionsAsync(Guid schoolYearId)
    {
        var result = await _dataProvider.GetPeriodsAsync(schoolYearId);
        return PartialView("_PeriodOptions", result);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> GetDepartmentsLoadViewAsync(Guid periodId)
    {
        var departments = await _dataProvider.GetDepartmentsWithLoadAsync(periodId);
        return PartialView("_DepartmentsLoadView", departments);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        try
        {
            var model = new CreateDepartmentModel { Name = "" };
            await LoadFacultiesIntoViewModel(model);
            return View(model);
        }
        catch (Exception)
        {
            return RedirectToActionPermanent("Error", "Home");
        }
    }

    private async Task LoadFacultiesIntoViewModel(CreateDepartmentModel model)
    {
        var faculties = await _dataProvider.GetFacultiesAsync();
        model.Faculties = faculties;
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> GetCareerSelectForFacultyAsync(Guid facultyId)
    {
        if (facultyId == Guid.Empty)
        {
            return BadRequest("Debe de proveer un id de facultad válido.");
        }

        var careers = await _dataProvider.GetCareersAsync(facultyId);
        return PartialView("_CareersSelect", careers);
    }

    [Authorize("Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateDepartmentModel model)
    {
        if (ModelState.IsValid)
        {
            if (await _dataProvider.ExistFacultyAsync(model.FacultyId))
            {
                var result = await _dataProvider.CreateDepartmentAsync(model);
                if (result)
                {
                    TempData["department-created"] = true;
                    return RedirectToActionPermanent("Index");
                }
                else
                {
                    ModelState.AddModelError("Error creando departamento", "Ha ocurrido un error creando el departamento.");
                }
            }
            else
            {
                ModelState.AddModelError("Error de facultad", "La facultad seleccionada no existe en el servidor.");
            }
        }

        await LoadFacultiesIntoViewModel(model);
        return View(model);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> EditAsync(Guid id)
    {
        try
        {
            var department = await _dataProvider.GetDepartmentAsync(id);
            var viewmodel = _mapper.Map<EditDepartmentModel>(department);
            viewmodel.Careers = await _dataProvider.GetCareersAsync(department.FacultyId);
            return View(viewmodel);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(EditDepartmentModel model)
    {
        if (ModelState.IsValid)
        {
            var datamodel = _mapper.Map<DepartmentModel>(model);
            try
            {
                var result = await _dataProvider.UpdateDepartmentAsync(datamodel);
                if (result)
                {
                    TempData["department-edited"] = true;
                    return RedirectToActionPermanent("Index");
                }

                ModelState.AddModelError("Error", "Ha ocurrido un error editando el departamento.");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        return View(model);
    }

    [Authorize("Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        try
        {
            if (await _dataProvider.ExistsDepartmentAsync(id))
            {
                var result = await _dataProvider.DeleteDepartmentAsync(id);
                if (result)
                {
                    TempData["department-deleted"] = true;
                    return Ok($"Se ha eliminado correctamente el departamento con id {id}.");
                }
                else
                {
                    return Problem($"Ha ocurrido un error eliminando el departmento con id {id}.");
                }
            }

            return NotFound($"No se ha encontrado el departamento con id {id}.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}