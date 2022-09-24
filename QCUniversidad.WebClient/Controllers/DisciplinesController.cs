using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;

namespace QCUniversidad.WebClient.Controllers
{
    [Authorize]
    public class DisciplinesController : Controller
    {
        private readonly IDataProvider _dataProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<DisciplinesController> _logger;
        private readonly NavigationSettings _navigationSettings;

        public DisciplinesController(IDataProvider dataProvider,
                                     IOptions<NavigationSettings> navSettings,
                                     IMapper mapper,
                                     ILogger<DisciplinesController> logger)
        {
            _dataProvider = dataProvider;
            _mapper = mapper;
            _logger = logger;
            _navigationSettings = navSettings.Value;
        }

        public async Task<IActionResult> IndexAsync(int page = 1)
        {
            _logger.LogRequest(HttpContext);
            try
            {
                _logger.LogInformation($"Loading total disciplines count.");
                var total = await _dataProvider.GetDisciplinesCountAsync();
                _logger.LogInformation("Exists {0} diciplines in total.", total);
                var pageIndex = page - 1 < 0 ? 0 : page - 1;
                var startingItemIndex = (pageIndex * _navigationSettings.ItemsPerPage);
                if (startingItemIndex < 0 || startingItemIndex >= total)
                {
                    startingItemIndex = 0;
                }
                _logger.LogInformation($"Loading disciplines starting in {startingItemIndex} and taking {_navigationSettings.ItemsPerPage}.");
                var disciplines = await _dataProvider.GetDisciplinesAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
                _logger.LogInformation($"Loaded {disciplines.Count} disciplines.");
                var totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
                var viewModel = new NavigationListViewModel<DisciplineModel>
                {
                    Items = disciplines,
                    CurrentPage = pageIndex + 1,
                    PagesCount = totalPages,
                    TotalItems = total
                };
                _logger.LogInformation("Returning view with {0} disciplines, in page {1}, total pages: {2}, total items: {3}", viewModel.ItemsCount, viewModel.CurrentPage, viewModel.PagesCount, viewModel.TotalItems);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Exception throwed {0}", ex.Message);
                return RedirectToActionPermanent("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            _logger.LogRequest(HttpContext);
            try
            {
                var model = new CreateDisciplineModel();
                await LoadDepartmentsIntoViewModel(model);
                _logger.LogInformation("Returning view with {0} departments list.", model.Departments.Count);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Exception throwed {0}", ex.Message);
                return RedirectToActionPermanent("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateDisciplineModel model)
        {
            _logger.LogRequest(HttpContext);
            if (ModelState.IsValid)
            {
                _logger.LogCheckModelExistence<DisciplinesController, DepartmentModel>(HttpContext, nameof(DepartmentModel), model.DepartmentId.ToString());
                if (await _dataProvider.ExistsDepartmentAsync(model.DepartmentId))
                {
                    var result = await _dataProvider.CreateDisciplineAsync(model);
                    if (result)
                    {
                        _logger.LogInformation($"Added discipline {model.Name} successfully.");
                        TempData["discipline-added"] = true;
                        return RedirectToActionPermanent("Index");
                    }
                    _logger.LogWarning($"Error while creating the discipline.");
                    ModelState.AddModelError("Error", "Error agregando la nueva disciplina."); 
                }
                else
                {
                    _logger.LogWarning($"The department with id {model.DepartmentId} does not exists.");
                    ModelState.AddModelError("Error", "No existe el departamento seleccionado.");
                }
            }
            _logger.LogWarning($"Model state is invalid with {ModelState.ErrorCount} errors.");
            await LoadDepartmentsIntoViewModel(model);
            return View(model);
        }

        private async Task LoadDepartmentsIntoViewModel(CreateDisciplineModel viewModel)
        {
            _logger.LogInformation("Loading list of departments");
            var departments = await _dataProvider.GetDepartmentsAsync();
            _logger.LogInformation("Loaded {0} departments.", departments.Count);
            viewModel.Departments = departments;
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(Guid id)
        {
            _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
            try
            {
                _logger.LogInformation($"Requested info for discipline with id {id}");
                var discipline = await _dataProvider.GetDisciplineAsync(id);
                _logger.LogInformation($"Loaded info for discipline {discipline.Name}");
                var viewmodel = _mapper.Map<EditDisciplineModel>(discipline);
                _logger.LogInformation("Returning view with discipline {0}.", viewmodel.Name);
                return View(viewmodel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in edit request of discipline with id {id}.");
                return RedirectToActionPermanent("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(EditDisciplineModel model)
        {
            _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
            if (ModelState.IsValid)
            {
                var datamodel = _mapper.Map<DisciplineModel>(model);
                try
                {
                    var result = await _dataProvider.UpdateDisciplineAsync(datamodel);
                    if (result)
                    {
                        _logger.LogInformation($"Updated discipline {model.Name} successfully.");
                        TempData["discipline-edited"] = true;
                        return RedirectToActionPermanent("Index");
                    }
                    else
                    {
                        _logger.LogWarning($"Error while updating the discipline.");
                        ModelState.AddModelError("Error", "Error agregando la nueva disciplina.");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToActionPermanent("Error", "Home");
                }
            }
            return View(model);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
            try
            {
                _logger.LogInformation($"Checking that the discipline with id {id} exists.");
                if (await _dataProvider.ExistsDisciplineAsync(id))
                {
                    _logger.LogInformation($"The discipline with id {id} exists.");
                    _logger.LogInformation($"Requesting delete the discipline with id {id}.");
                    var result = await _dataProvider.DeleteDisciplineAsync(id);
                    if (result)
                    {
                        _logger.LogInformation($"The discipline with id {id} was eliminated successfully.");
                        TempData["discipline-deleted"] = true;
                        return Ok();
                    }
                }
                _logger.LogInformation($"The discipline with id {id} does not exists, returning NotFound result.");
                return NotFound($"No se ha encontrado la disciplina con id {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on deleting discipline with id {id}, returning Problem result.");
                return Problem(ex.Message);
            }
        }
    }
}