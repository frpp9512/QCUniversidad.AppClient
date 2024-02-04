using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.WebClient.Data.Contexts;
using QCUniversidad.WebClient.Models.Index;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Models.Statistics;
using QCUniversidad.WebClient.Models.Teachers;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;
using System.Diagnostics;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Auth")]
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
        try
        {
            Models.SchoolYears.SchoolYearModel schoolYear = await _dataProvider.GetCurrentSchoolYear();
            if (User.IsAdmin())
            {
                model = new IndexViewModel
                {
                    SchoolYear = schoolYear
                };
                return View(model);
            }

            if (User.IsDepartmentManager())
            {
                model = new IndexViewModel
                {
                    Department = await _dataProvider.GetDepartmentAsync(User.GetDepartmentId()),
                    SchoolYear = schoolYear
                };
                return View(model);
            }

            if (User.IsPlanner())
            {
                model = new IndexViewModel
                {
                    Faculty = await _dataProvider.GetFacultyAsync(User.GetFacultyId()),
                    SchoolYear = schoolYear
                };
                return View(model);
            }

            return RedirectToAction("Error");
        }
        catch
        {
            return RedirectToAction("Error");
        }
    }

    public IActionResult Fa()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetGlobalStatisticsAsync()
    {
        if (User.IsAdmin())
        {
            try
            {
                IList<StatisticItemModel> statistics = await _dataProvider.GetGlobalStatisticsAsync();
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
                if (User.IsDepartmentManager())
                {
                    Guid departmentId = User.GetDepartmentId();
                    IList<StatisticItemModel> statistics = await _dataProvider.GetGlobalStatisticsForDepartmentAsync(departmentId);
                    return PartialView("_ICardSet", statistics);
                }
                else
                {
                    Guid facultyId = User.GetFacultyId();
                    List<StatisticItemModel> statistics = [];
                    return PartialView("_ICardSet", statistics);
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetWeekBirthdaysAsync()
    {
        IList<BirthdayTeacherModel> birthdays;
        if (User.IsDepartmentManager())
        {
            Guid departmentId = User.GetDepartmentId();
            _ = await _dataProvider.GetBirthdayTeachersForCurrentMonthAsync(departmentId, "department");
        }

        if (User.IsPlanner())
        {
            Guid facultyId = User.GetFacultyId();
            birthdays = await _dataProvider.GetBirthdayTeachersForCurrentMonthAsync(facultyId, "faculty");
        }
        else
        {
            birthdays = await _dataProvider.GetBirthdayTeachersForCurrentMonthAsync(new Guid(), "global");
        }

        return PartialView("_BirthdayCard", birthdays);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}