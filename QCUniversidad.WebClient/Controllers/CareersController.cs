using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Contracts;
using QCUniversidad.WebClient.Services.Platform;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Auth")]
public class CareersController(ICareersDataProvider dataProvider,
                               ISchoolYearDataProvider schoolYearDataProvider,
                               IFacultiesDataProvider facultiesDataProvider,
                               IMapper mapper,
                               ILogger<CareersController> logger,
                               IOptions<NavigationSettings> settings) : Controller
{
    private readonly ICareersDataProvider _careersDataProvider = dataProvider;
    private readonly ISchoolYearDataProvider _schoolYearDataProvider = schoolYearDataProvider;
    private readonly IFacultiesDataProvider _facultiesDataProvider = facultiesDataProvider;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CareersController> _logger = logger;
    private readonly NavigationSettings _navigationSettings = settings.Value;

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page = 0)
    {
        _logger.LogInformation($"Requested {HttpContext.Request.Path} - {HttpContext.Request.Method}");
        try
        {
            int total = await _careersDataProvider.GetCareersCountAsync();
            int pageIndex = page - 1 < 0 ? 0 : page - 1;
            int startingItemIndex = pageIndex * _navigationSettings.ItemsPerPage;
            if (startingItemIndex < 0 || startingItemIndex >= total)
            {
                startingItemIndex = 0;
            }

            IList<CareerModel> careers = await _careersDataProvider.GetCareersAsync(startingItemIndex, _navigationSettings.ItemsPerPage);
            int totalPages = (int)Math.Ceiling((double)total / _navigationSettings.ItemsPerPage);
            NavigationListViewModel<CareerModel> viewModel = new()
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
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return RedirectToAction("Error", "Home");
        }

        try
        {
            CareerModel career = await _careersDataProvider.GetCareerAsync(id);
            Models.SchoolYears.SchoolYearModel schoolYear = await _schoolYearDataProvider.GetSchoolYearAsync(id);
            ViewData["schoolYear"] = schoolYear;
            return View(career);
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        _logger.LogRequest(HttpContext);
        try
        {
            CreateCareerModel model = new() { Name = "" };
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
        IList<FacultyModel> faculties = await _facultiesDataProvider.GetFacultiesAsync();
        model.Faculties = faculties;
    }

    [Authorize("Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCareerModel model)
    {
        _logger.LogRequest(HttpContext);
        if (ModelState.IsValid)
        {
            _logger.LogCheckModelExistence<CareersController, FacultyModel>(HttpContext, model.FacultyId.ToString());
            if (await _facultiesDataProvider    .ExistFacultyAsync(model.FacultyId))
            {
                _logger.LogCreateModelRequest<CareersController, CareerModel>(HttpContext);
                bool result = await _careersDataProvider.CreateCareerAsync(model);
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

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> EditAsync(Guid id)
    {
        _logger.LogRequest(HttpContext);
        try
        {
            _logger.LogModelSetLoading<CareersController, CareerModel>(HttpContext, id);
            CareerModel model = await _careersDataProvider.GetCareerAsync(id);
            EditCareerModel editModel = _mapper.Map<EditCareerModel>(model);
            return View(editModel);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(EditCareerModel model)
    {
        _logger.LogRequest(HttpContext);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            _logger.LogEditModelRequest<CareersController, CareerModel>(HttpContext, model.Id);
            bool result = await _careersDataProvider.UpdateCareerAsync(_mapper.Map<CareerModel>(model));
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
            return RedirectToAction("Error", "Home");
        }

        return View(model);
    }

    [Authorize("Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        _logger.LogRequest(HttpContext);
        try
        {
            _logger.LogCheckModelExistence<CareersController, CareerModel>(HttpContext, id);
            if (!await _careersDataProvider.ExistsCareerAsync(id))
            {
                return NotFound($"No se ha encontrado el departamento con id {id}.");
            }

            _logger.LogDeleteModelRequest<CareersController, CareerModel>(HttpContext, id);
            bool result = await _careersDataProvider.DeleteCareerAsync(id);
            if (!result)
            {
                return NotFound($"No se ha encontrado el departamento con id {id}.");
            }

            _logger.LogDeleteModelRequest<CareersController, CareerModel>(HttpContext, id);
            TempData["career-deleted"] = true;
            return Ok($"Se ha eliminado correctamente la carrera con id {id}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error on deleting career with id {id}, returning Problem result.");
            return Problem(ex.Message);
        }
    }
}