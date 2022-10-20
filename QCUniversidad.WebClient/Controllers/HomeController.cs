using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.WebClient.Data.Contexts;
using QCUniversidad.WebClient.Models;
using QCUniversidad.WebClient.Models.Index;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;
using SmartB1t.Security.WebSecurity.Local;
using System.Diagnostics;

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
            if (User.IsAdmin())
            {
                model = new IndexViewModel();
            }
            else
            {
                model = new IndexViewModel 
                {
                    Department = await _dataProvider.GetDepartmentAsync(User.GetDepartmentId())
                };
            }
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
                return Ok("<h1>Usuario administrador, pendiente para definir estadísticas</h1>");
            }
            else
            {
                try
                {
                    var departmentId = User.GetDepartmentId();
                    var statistics = await _dataProvider.GetGlobalStatisticsForDepartment(departmentId);
                    return PartialView("_ICardSet", statistics);
                }
                catch (Exception ex)
                {
                    return Problem(ex.Message);
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}