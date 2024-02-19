using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Contracts;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Auth")]
public class DepartmentsController(IDepartmentsDataProvider dataProvider,
                                   ISchoolYearDataProvider schoolYearDataProvider,
                                   IDisciplinesDataProvider disciplinesDataProvider,
                                   ITeachersDataProvider teachersDataProvider,
                                   ICareersDataProvider careersDataProvider,
                                   IPeriodsDataProvider periodsDataProvider,
                                   IFacultiesDataProvider facultiesDataProvider,
                                   IOptions<NavigationSettings> navSettingsOptions,
                                   IMapper mapper) : Controller
{
    private readonly IDepartmentsDataProvider _departmentsDataProvider = dataProvider;
    private readonly ISchoolYearDataProvider _schoolYearDataProvider = schoolYearDataProvider;
    private readonly IDisciplinesDataProvider _disciplinesDataProvider = disciplinesDataProvider;
    private readonly ITeachersDataProvider _teachersDataProvider = teachersDataProvider;
    private readonly ICareersDataProvider _careersDataProvider = careersDataProvider;
    private readonly IPeriodsDataProvider _periodsDataProvider = periodsDataProvider;
    private readonly IFacultiesDataProvider _facultiesDataProvider = facultiesDataProvider;
    private readonly IMapper _mapper = mapper;
    private readonly NavigationSettings _navigationSettings = navSettingsOptions.Value;

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page = 1)
    {
        try
        {
            int total = await _departmentsDataProvider.GetDepartmentsCountAsync();
            int pageIndex = page - 1 < 0 ? 0 : page - 1;
            int startingItemIndex = pageIndex * _navigationSettings.ItemsPerPage;
            if (startingItemIndex < 0 || startingItemIndex >= total)
            {
                startingItemIndex = 0;
            }

            IList<DepartmentModel> departments = await _departmentsDataProvider.GetDepartmentsAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
            int totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
            NavigationListViewModel<DepartmentModel> viewModel = new()
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
            DepartmentModel department = await _departmentsDataProvider.GetDepartmentAsync(id);
            Models.SchoolYears.SchoolYearModel schoolYear = await _schoolYearDataProvider.GetCurrentSchoolYear();
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
            IList<Models.Disciplines.DisciplineModel> disciplines = await _disciplinesDataProvider.GetDisciplinesAsync(departmentId);
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
            IList<Models.Teachers.TeacherModel> teachers = await _teachersDataProvider.GetTeachersOfDepartmentAsync(departmentId);
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
            IList<Models.Careers.CareerModel> careers = await _careersDataProvider.GetCareersForDepartmentAsync(departmentId);
            return PartialView("_DepartmentCareers", careers);
        }
        catch (Exception ex) { return Problem(detail: ex.Message); }
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> LoadViewAsync()
    {
        IList<Models.SchoolYears.SchoolYearModel> schoolYears = await _schoolYearDataProvider.GetSchoolYearsAsync();
        return View(schoolYears);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> GetPeriodOptionsAsync(Guid schoolYearId)
    {
        IList<Models.Periods.PeriodModel> result = await _periodsDataProvider.GetPeriodsAsync(schoolYearId);
        return PartialView("_PeriodOptions", result);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> GetDepartmentsLoadViewAsync(Guid periodId)
    {
        IList<DepartmentModel> departments = await _departmentsDataProvider.GetDepartmentsWithLoadAsync(periodId);
        return PartialView("_DepartmentsLoadView", departments);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        try
        {
            CreateDepartmentModel model = new() { Name = "" };
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
        IList<Models.Faculties.FacultyModel> faculties = await _facultiesDataProvider.GetFacultiesAsync();
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

        IList<Models.Careers.CareerModel> careers = await _careersDataProvider.GetCareersAsync(facultyId);
        return PartialView("_CareersSelect", careers);
    }

    [Authorize("Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateDepartmentModel model)
    {
        if (ModelState.IsValid)
        {
            if (await _facultiesDataProvider.ExistFacultyAsync(model.FacultyId))
            {
                bool result = await _departmentsDataProvider.CreateDepartmentAsync(model);
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
            DepartmentModel department = await _departmentsDataProvider.GetDepartmentAsync(id);
            EditDepartmentModel viewmodel = _mapper.Map<EditDepartmentModel>(department);
            viewmodel.Careers = await _careersDataProvider.GetCareersAsync(department.FacultyId);
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
            DepartmentModel datamodel = _mapper.Map<DepartmentModel>(model);
            try
            {
                bool result = await _departmentsDataProvider.UpdateDepartmentAsync(datamodel);
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
            if (!await _departmentsDataProvider.ExistsDepartmentAsync(id))
            {
                return NotFound($"No se ha encontrado el departamento con id {id}.");
            }

            bool result = await _departmentsDataProvider.DeleteDepartmentAsync(id);
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
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}