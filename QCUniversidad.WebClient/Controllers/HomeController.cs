using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.WebClient.Data.Contexts;
using QCUniversidad.WebClient.Models;
using QCUniversidad.WebClient.Models.Index;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Models.Teachers;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;
using SmartB1t.Security.WebSecurity.Local;
using System.Diagnostics;
using System.Drawing;

namespace QCUniversidad.WebClient.Controllers
{
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
        public async Task<IActionResult> GetTeachersChartAsync()
        {
            IList<TeacherModel> teachers;
            if (User.IsAdmin())
            {
                teachers = await _dataProvider.GetTeachersAsync(0, 0);
            }
            else
            {
                var departmentId = User.GetDepartmentId();
                teachers = await _dataProvider.GetTeachersOfDepartmentAsync(departmentId);
            }
            var chartModel = ModelListCharter.GetChartModel(ChartType.Doughnut,
                                                            new List<TeacherCategory>((TeacherCategory[])Enum.GetValues(typeof(TeacherCategory))), 
                                                            e => teachers.Count(t => t.Category == e),
                                                            e => e.ToString(),
                                                            title: "Profesores por categoría",
                                                            showXScale: false,
                                                            showYScale: false,
                                                            legendPosition: ChartLegendPosition.Left);
            return Ok(chartModel.GetJson());
        }

        [HttpGet]
        public async Task<IActionResult> GetTeachersChartByContractTypeAsync()
        {
            IList<TeacherModel> teachers;
            if (User.IsAdmin())
            {
                teachers = await _dataProvider.GetTeachersAsync(0, 0);
            }
            else
            {
                var departmentId = User.GetDepartmentId();
                teachers = await _dataProvider.GetTeachersOfDepartmentAsync(departmentId);
            }
            var chartModel = ModelListCharter.GetChartModel(ChartType.Doughnut,
                                                            new List<TeacherContractType>((TeacherContractType[])Enum.GetValues(typeof(TeacherContractType))),
                                                            e => teachers.Count(t => t.ContractType == e),
                                                            e => e.ToString(),
                                                            title: "Profesores por tipo de contrato",
                                                            showXScale: false,
                                                            showYScale: false,
                                                            legendPosition: ChartLegendPosition.Left);
            return Ok(chartModel.GetJson());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}