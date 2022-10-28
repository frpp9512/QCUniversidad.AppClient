using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.LoadDistribution;
using QCUniversidad.WebClient.Models.LoadItem;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;

namespace QCUniversidad.WebClient.Controllers;

[Authorize(Roles = "Administrador,Jefe de departamento")]
public class LoadDistributionController : Controller
{
    private readonly IDataProvider _dataProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<PlanningController> _logger;
    private readonly NavigationSettings _navigationSettings;

    public LoadDistributionController(IDataProvider dataProvider, IMapper mapper, IOptions<NavigationSettings> navOptions, ILogger<PlanningController> logger)
    {
        _dataProvider = dataProvider;
        _mapper = mapper;
        _logger = logger;
        _navigationSettings = navOptions.Value;
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync(Guid? departmentId = null, Guid? schoolYearId = null)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("SelectDepartment");
        }
        if (!User.IsAdmin() && schoolYearId is not null)
        {
            schoolYearId = null;
        }
        var workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        var schoolYear = schoolYearId is null ? await _dataProvider.GetCurrentSchoolYear() : await _dataProvider.GetSchoolYearAsync(schoolYearId.Value);
        var department = await _dataProvider.GetDepartmentAsync(workingDepartment);
        var courses = await _dataProvider.GetCoursesForDepartment(workingDepartment, schoolYear.Id);
        var periods = await _dataProvider.GetPeriodsAsync(schoolYear.Id);
        var model = new LoadDistributionIndexModel
        {
            Department = department,
            Courses = courses,
            SchoolYear = schoolYear,
            Periods = periods
        };
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> SelectDepartmentAsync()
    {
        var schoolYears = await _dataProvider.GetSchoolYearsAsync();
        var departments = await _dataProvider.GetDepartmentsAsync();
        var model = new SelectDepartmentViewModel 
        {
            Departments = departments,
            SchoolYears = schoolYears
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetPeriodOptionsAsync(Guid courseId, Guid? schoolYearId, Guid? departmentId)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("Error", "Home");
        }
        var workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        try
        {
            var result = await _dataProvider.GetPeriodsAsync(schoolYearId);
            return PartialView("_PeriodOptions", result);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPlanningItemsViewAsync(Guid periodId, Guid? departmentId, Guid? courseId = null)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("Error", "Home");
        }
        var workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        try
        {
            var result = await _dataProvider.GetTeachingPlanItemsOfDepartmentOnPeriodAsync(workingDepartment, periodId, courseId);
            return PartialView("_SimplifiedPlanningListView", result);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTeachersForDepartmentInPeriodAsync(Guid periodId, Guid? departmentId = null)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("Error", "Home");
        }
        var workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        try
        {
            var depTeachers = await _dataProvider.GetTeachersOfDepartmentForPeriodAsync(workingDepartment, periodId);
            var supportTeachers = await _dataProvider.GetSupportTeachersAsync(workingDepartment, periodId);
            var model = new TeachersViewModel
            {
                DepartmentsTeacher = depTeachers,
                SupportTeachers = supportTeachers
            };
            return PartialView("_TeachersWithLoadView", model);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAddLoadModalAsync(Guid planItemId, Guid? departmentId = null, Guid? disciplineId = null)
    {
        if (planItemId == Guid.Empty)
        {
            return BadRequest("The plan item should not be null");
        }
        if (User.IsAdmin() && departmentId is null)
        {
            return BadRequest("If you're logged in as admin you should provide a department id.");
        }
        var workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        var loadItem = await _dataProvider.GetTeachingPlanItemAsync(planItemId);
        var teachers = await _dataProvider.GetTeachersOfDepartmentNotAssignedToLoadItemAsync(workingDepartment, planItemId, disciplineId);

        var viewModel = new AddLoadModalModel
        {
            PlanItem = loadItem,
            Teachers = teachers,
            MaxValue = loadItem.TotalHoursPlanned - loadItem.TotalLoadCovered ?? 0
        };

        return PartialView("_AddLoadModal", viewModel);
    }

    [HttpPut]
    public async Task<IActionResult> SetTeacherLoadAsync([FromBody] CreateLoadItemModel model)
    {
        if (model is not null)
        {
            try
            {
                var result = await _dataProvider.SetLoadItemAsync(model);
                return result ? Ok(result) : Problem();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        return BadRequest("The model should not be null.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteLoadItemAsync(Guid loadItemId)
    {
        if(loadItemId == Guid.Empty)
        {
            return BadRequest();
        }
        try
        {
            var result = await _dataProvider.DeleteLoadItemAsync(loadItemId);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTeacherLoadDetailsModalContentAsync(Guid teacherId, Guid periodId)
    {
        if (teacherId == Guid.Empty)
        {
            return BadRequest("Should provide a teacher id.");
        }
        if (periodId == Guid.Empty)
        {
            return BadRequest("Should provide a period id.");
        }
        try
        {
            var teacher = await _dataProvider.GetTeacherAsync(teacherId);
            var loadItems = await _dataProvider.GetTeacherLoadItemsInPeriodAsync(teacherId, periodId);
            var model = new TeacherLoadViewModel 
            { 
                Teacher = teacher,
                LoadItems = loadItems
            };
            return PartialView("_TeacherLoadViewContent", model);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}