using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.LoadDistribution;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public async Task<IActionResult> IndexAsync(Guid? departmentId = null)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("SelectDepartment");
        }
        Guid workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        var department = await _dataProvider.GetDepartmentAsync(workingDepartment);
        var schoolYears = await _dataProvider.GetSchoolYearsForDepartment(departmentId.Value);
        var model = new LoadDistributionIndexModel 
        {
            Department = department,
            SchoolYears = schoolYears
        };
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> SelectDepartmentAsync()
    {
        var departments = await _dataProvider.GetDepartmentsAsync();
        return View(departments);
    }

    [HttpGet]
    public async Task<IActionResult> GetPeriodOptionsAsync(Guid schoolYearId, Guid? departmentId)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("Error", "Home");
        }
        var workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        try
        {
            var result = await _dataProvider.GetPeriodsOfSchoolYearForDepartment(schoolYearId, workingDepartment);
            return PartialView("_PeriodOptions", result);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPlanningItemsViewAsync(Guid periodId, Guid? departmentId)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("Error", "Home");
        }
        var workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        try
        {
            var result = await _dataProvider.GetTeachingPlanItemsOfDepartmentOnPeriod(workingDepartment, periodId);
            return PartialView("_SimplifiedPlanningListView", result);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }
}