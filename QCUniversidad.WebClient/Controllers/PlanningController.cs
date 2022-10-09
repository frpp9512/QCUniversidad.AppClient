using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Controllers
{
    [Authorize]
    public class PlanningController : Controller
    {
        private readonly IDataProvider _dataProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<PlanningController> _logger;
        private readonly NavigationSettings _navigationSettings;

        public PlanningController(IDataProvider dataProvider, IMapper mapper, IOptions<NavigationSettings> navOptions, ILogger<PlanningController> logger)
        {
            _dataProvider = dataProvider;
            _mapper = mapper;
            _logger = logger;
            _navigationSettings = navOptions.Value;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}