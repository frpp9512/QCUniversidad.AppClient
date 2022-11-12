using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.Api.Shared.Extensions;
using QCUniversidad.WebClient.Data.Contexts;
using QCUniversidad.WebClient.Models.Index;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Models.Teachers;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;
using System.Diagnostics;

namespace QCUniversidad.WebClient.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly WebDataContext _context;
    private readonly IDataProvider _dataProvider;

    public HomeController(ILogger<HomeController> logger, WebDataContext context, IDataProvider dataProvider)
    {
        _logger = logger;
        _context = context;
        _dataProvider = dataProvider;
    }

    public async Task<IActionResult> IndexAsync()
    {
        IndexViewModel model;
        var schoolYear = await _dataProvider.GetCurrentSchoolYear();
        model = User.IsAdmin()
            ? new IndexViewModel
            {
                SchoolYear = schoolYear
            }
            : new IndexViewModel
            {
                Department = await _dataProvider.GetDepartmentAsync(User.GetDepartmentId()),
                SchoolYear = schoolYear
            };
        return View(model);
    }

    public IActionResult Fa()
        => View();

    public IActionResult Privacy() => View();

    [HttpGet]
    public async Task<IActionResult> GetGlobalStatisticsAsync()
    {
        if (User.IsAdmin())
        {
            try
            {
                var statistics = await _dataProvider.GetGlobalStatisticsAsync();
                return PartialView("_ICardSet", statistics);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        else
        {
            try
            {
                var departmentId = User.GetDepartmentId();
                var statistics = await _dataProvider.GetGlobalStatisticsForDepartmentAsync(departmentId);
                return PartialView("_ICardSet", statistics);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }

    [HttpGet]
    [Authorize(Roles = "Jefe de departamento")]
    public async Task<IActionResult> GetWeekBirthdaysAsync()
    {
        var departmentId = User.GetDepartmentId();
        var birthdays = await _dataProvider.GetBirthdayTeachersForCurrentMonthAsync(departmentId);
        return PartialView("_BirthdayCard", birthdays);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}