using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Controllers
{
    [Authorize]
    public class CareersController : Controller
    {
        private readonly IDataProvider _dataProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<CareersController> _logger;
        private readonly NavigationSettings _navigationSettings;

        public CareersController(IDataProvider dataProvider, IMapper mapper, ILogger<CareersController> logger, IOptions<NavigationSettings> settings)
        {
            _dataProvider = dataProvider;
            _mapper = mapper;
            _logger = logger;
            _navigationSettings = settings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(int page = 0)
        {
            _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
            try
            {
                var total = await _dataProvider.GetCareersCountAsync();
                var pageIndex = page - 1 < 0 ? 0 : page - 1;
                var startingItemIndex = (pageIndex * _navigationSettings.ItemsPerPage);
                if (startingItemIndex < 0 || startingItemIndex >= total)
                {
                    startingItemIndex = 0;
                }
                var careers = await _dataProvider.GetCareersAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
                var totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
                var viewModel = new NavigationListViewModel<CareerModel>
                {
                    Items = careers,
                    CurrentPage = pageIndex + 1,
                    PagesCount = totalPages,
                    TotalItems = total
                };
                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToActionPermanent("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            _logger.LogRequest(HttpContext);
            try
            {
                var model = new CreateCareerModel();
                await LoadFacultiesIntoCreateModel(model);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        private async Task LoadFacultiesIntoCreateModel(CreateCareerModel model)
        {
            _logger.LogModelSetLoading<CareersController, FacultyModel>(HttpContext);
            var faculties = await _dataProvider.GetFacultiesAsync();
            model.Faculties = faculties;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCareerModel model)
        {
            _logger.LogRequest(HttpContext);
            if (ModelState.IsValid)
            {
                _logger.LogCheckModelExistence<CareersController, FacultyModel>(HttpContext, model.FacultyId.ToString());
                if (await _dataProvider.ExistFacultyAsync(model.FacultyId))
                {
                    _logger.LogCreateModelRequest<CareersController, CareerModel>(HttpContext);
                    var result = await _dataProvider.CreateCareerAsync(model);
                    if (result)
                    {
                        _logger.LogModelCreated<CareersController, CareerModel>(HttpContext);
                        TempData["career-created"] = true;
                        return RedirectToActionPermanent("Index");
                    }
                    else
                    {
                        _logger.LogErrorCreatingModel<CareersController, CareerModel>(HttpContext);
                        ModelState.AddModelError("Error creando carrera", "Ha ocurrido un error creando la carrera.");
                    }
                }
                else
                {
                    _logger.LogModelNotExist<CareersController, FacultyModel>(HttpContext, model.Faculty.Id.ToString());
                    ModelState.AddModelError("Error de facultad", "La facultad seleccionada no existe en el servidor.");
                }
            }
            await LoadFacultiesIntoCreateModel(model);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(Guid id)
        {
            _logger.LogRequest(HttpContext);
            try
            {
                _logger.LogModelSetLoading<CareersController, CareerModel>(HttpContext, id);
                var model = await _dataProvider.GetCareerAsync(id);
                var editModel = _mapper.Map<EditCareerModel>(model);
                return View(editModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(EditCareerModel model)
        {
            _logger.LogRequest(HttpContext);
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogEditModelRequest<CareersController, CareerModel>(HttpContext, model.Id);
                    var result = await _dataProvider.UpdateCareerAsync(_mapper.Map<CareerModel>(model));
                    if (result)
                    {
                        _logger.LogModelEdited<CareersController, CareerModel>(HttpContext, model.Id);
                        TempData["career-edited"] = true;
                        return RedirectToActionPermanent("Index");
                    }
                    else
                    {
                        _logger.LogErrorEditingModel<CareersController, CareerModel>(HttpContext, model.Id);
                        ModelState.AddModelError("Error", "Error editando la carrera.");
                    }
                }
                catch (Exception)
                {
                    return RedirectToActionPermanent("Error", "Home");
                }
            }
            return View(model);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _logger.LogRequest(HttpContext);
            try
            {
                _logger.LogCheckModelExistence<CareersController, CareerModel>(HttpContext, id);
                if (await _dataProvider.ExistsCareerAsync(id))
                {
                    _logger.LogDeleteModelRequest<CareersController, CareerModel>(HttpContext, id);
                    var result = await _dataProvider.DeleteCareerAsync(id);
                    if (result)
                    {
                        _logger.LogDeleteModelRequest<CareersController, CareerModel>(HttpContext, id);
                        TempData["career-deleted"] = true;
                        return Ok($"Se ha eliminado correctamente la carrera con id {id}.");
                    }
                }
                return NotFound($"No se ha encontrado el departamento con id {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on deleting career with id {id}, returning Problem result.");
                return Problem(ex.Message);
            }
        }
    }
}