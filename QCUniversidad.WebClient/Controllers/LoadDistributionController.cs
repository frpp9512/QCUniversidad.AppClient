using AutoMapper;
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
        public async Task<IActionResult> IndexAsync()
        {
            return View();
        }
    }
}
