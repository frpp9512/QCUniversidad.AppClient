using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Services.Data;

namespace QCUniversidad.WebClient.Controllers;

[Authorize(Roles = "Administrador,Planificador")]
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

    [HttpGet]
    public async Task<IActionResult> Index(Guid? courseSelected = null, Guid? periodSelected = null)
    {
        var model = new PlanningIndexModel
        {
            Courses = (await _dataProvider.GetCoursesAsync()).OrderByDescending(s => s.Starts).ToList()
        };
        if (courseSelected is not null && courseSelected != Guid.Empty)
        {
            model.CourseSelected = courseSelected;
        }
        if (periodSelected is not null && periodSelected != Guid.Empty)
        {
            model.PeriodSelected = periodSelected;
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetPlanningViewForPeriodAsync(Guid periodId)
    {
        try
        {
            var models = await _dataProvider.GetTeachingPlanItemsAsync(periodId, 0, 0);
            return PartialView("_PlanningListView", models);
        }
        catch (Exception)
        {
            return Problem();
        }
    }

    [HttpGet]
    public async Task<IActionResult> CreatePlanningItemAsync(Guid periodId)
    {
        try
        {
            if (periodId == Guid.Empty)
            {
                return RedirectToAction("Error", "Home");
            }
            if (!await _dataProvider.ExistsPeriodAsync(periodId))
            {
                return RedirectToAction("Error", "Home");
            }
            var viewModel = await GetCreateViewModel(periodId);

            return View(viewModel);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    private async Task<CreateTeachingPlanItemModel> GetCreateViewModel(Guid periodId)
    {
        var periodModel = await _dataProvider.GetPeriodAsync(periodId);
        var subjects = await _dataProvider.GetSubjectsForCourseAsync(periodModel.CourseId);
        var course = await _dataProvider.GetCourseAsync(periodModel.CourseId);
        var viewModel = new CreateTeachingPlanItemModel
        {
            Subjects = subjects,
            PeriodId = periodId,
            Period = periodModel,
            CourseId = periodModel.CourseId,
            Course = course
        };
        return viewModel;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePlanningItemAsync(CreateTeachingPlanItemModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.GroupsAmount >= 0)
            {
                var planItem = _mapper.Map<TeachingPlanItemModel>(model);
                var result = await _dataProvider.CreateTeachingPlanItemAsync(planItem);
                if (result)
                {
                    TempData["planItem-created"] = true;
                    return RedirectToAction("Index", new { courseSelected = model.CourseId, periodSelected = model.PeriodId });
                }
            }
            else
            {
                ModelState.AddModelError("GroupsAmount", "Debe de definir al menos un grupo.");
            }
        }
        model.Period = await _dataProvider.GetPeriodAsync(model.PeriodId);
        model.Subjects = await _dataProvider.GetSubjectsForCourseAsync(model.Period.CourseId);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditPlanningItemAsync(Guid id)
    {
        if (id == Guid.Empty || !await _dataProvider.ExistsTeachingPlanItemAsync(id))
        {
            return RedirectToAction("Error", "Home");
        }
        var editModel = await GetEditViewModel(id);
        return View(editModel);
    }

    private async Task<EditTeachingPlanItemModel> GetEditViewModel(Guid id)
    {
        var planningItem = await _dataProvider.GetTeachingPlanItemAsync(id);
        var periodModel = await _dataProvider.GetPeriodAsync(planningItem.PeriodId);
        var subjects = await _dataProvider.GetSubjectsForCourseAsync(periodModel.CourseId);
        var course = await _dataProvider.GetCourseAsync(periodModel.CourseId);
        var viewModel = _mapper.Map<EditTeachingPlanItemModel>(planningItem);
        viewModel.Subjects = subjects;
        viewModel.PeriodId = planningItem.PeriodId;
        viewModel.Period = periodModel;
        viewModel.CourseId = course.Id;
        viewModel.Course = course;
        return viewModel;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPlanningItemAsync(EditTeachingPlanItemModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.GroupsAmount >= 0)
            {
                var planItem = _mapper.Map<TeachingPlanItemModel>(model);
                var result = await _dataProvider.UpdateTeachingPlanItemAsync(planItem);
                if (result)
                {
                    TempData["planItem-edited"] = true;
                    return RedirectToAction("Index", new { courseSelected = model.CourseId, periodSelected = model.PeriodId });
                }
            }
            else
            {
                ModelState.AddModelError("GroupsAmount", "Debe de definir al menos un grupo.");
            }
        }
        model.Subjects = await _dataProvider.GetSubjectsForCourseAsync(model.CourseId.Value);
        model.Period = await _dataProvider.GetPeriodAsync(model.PeriodId);
        model.Course = await _dataProvider.GetCourseAsync(model.CourseId.Value);
        return View(model);
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePlanningItemAsync(Guid id)
    {
        if (id != Guid.Empty && await _dataProvider.ExistsTeachingPlanItemAsync(id))
        {
            var result = await _dataProvider.DeleteTeachingPlanItemAsync(id);
            if (result)
            {
                TempData["planItem-edited"] = true;
                return Ok(result);
            }
            else
            {
                return Problem();
            }
        }
        return BadRequest("El id debe de ser correcto.");
    }
}