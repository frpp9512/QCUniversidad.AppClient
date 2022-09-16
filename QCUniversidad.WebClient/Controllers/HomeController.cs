using Microsoft.AspNetCore.Mvc;
using QCUniversidad.WebClient.Data.Contexts;
using QCUniversidad.WebClient.Models;
using SmartB1t.Security.WebSecurity.Local;
using System.Diagnostics;

namespace QCUniversidad.WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebDataContext _context;

        public HomeController(ILogger<HomeController> logger, WebDataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //var roles = new List<Role> 
            //{
            //    new Role 
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Administrador",
            //        Description = "Gestiona los datos de las facultades.",
            //        Active = true
            //    },
            //    new Role
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Jefe de departamento",
            //        Description = "Gestiona la distribución de carga.",
            //        Active = true
            //    },
            //    new Role
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Consultor",
            //        Description = "Consulta la información del sistema.",
            //        Active = true
            //    }
            //};
            //_context.Roles.AddRange(roles);
            //_context.SaveChanges();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}