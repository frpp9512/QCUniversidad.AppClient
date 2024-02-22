using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.WebClient.Data.Contexts;
using QCUniversidad.WebClient.Models.Index;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Models.Statistics;
using QCUniversidad.WebClient.Models.Teachers;
using QCUniversidad.WebClient.Services.Contracts;
using QCUniversidad.WebClient.Services.Platform;
using System.Diagnostics;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Auth")]
public class HomeController(ISchoolYearDataProvider schoolYearDataProvider,
                            IDepartmentsDataProvider departmentsDataProvider,
                            IFacultiesDataProvider facultiesDataProvider,
                            IStatisticsDataProvider statisticsDataProvider,
                            ITeachersDataProvider teachersDataProvider) : Controller
{
    private readonly ISchoolYearDataProvider _schoolYearDataProvider = schoolYearDataProvider;
    private readonly IDepartmentsDataProvider _departmentsDataProvider = departmentsDataProvider;
    private readonly IFacultiesDataProvider _facultiesDataProvider = facultiesDataProvider;
    private readonly IStatisticsDataProvider _statisticsDataProvider = statisticsDataProvider;
    private readonly ITeachersDataProvider _teachersDataProvider = teachersDataProvider;

    public async Task<IActionResult> IndexAsync()
    {
        IndexViewModel model;
        try
        {
            Models.SchoolYears.SchoolYearModel schoolYear = await _schoolYearDataProvider.GetCurrentSchoolYear();
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
                    Department = await _departmentsDataProvider.GetDepartmentAsync(User.GetDepartmentId()),
                    SchoolYear = schoolYear
                };
                return View(model);
            }

            if (User.IsPlanner())
            {
                model = new IndexViewModel
                {
                    Faculty = await _facultiesDataProvider.GetFacultyAsync(User.GetFacultyId()),
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
                IList<StatisticItemModel> statistics = await _statisticsDataProvider.GetGlobalStatisticsAsync();
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
                    IList<StatisticItemModel> statistics = await _statisticsDataProvider.GetGlobalStatisticsForDepartmentAsync(departmentId);
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
            _ = await _teachersDataProvider.GetBirthdayTeachersForCurrentMonthAsync(departmentId, "department");
        }

        if (User.IsPlanner())
        {
            Guid facultyId = User.GetFacultyId();
            birthdays = await _teachersDataProvider.GetBirthdayTeachersForCurrentMonthAsync(facultyId, "faculty");
        }
        else
        {
            birthdays = await _teachersDataProvider.GetBirthdayTeachersForCurrentMonthAsync(new Guid(), "global");
        }

        return PartialView("_BirthdayCard", birthdays);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}