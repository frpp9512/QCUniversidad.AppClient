using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Models.SchoolYears;
using QCUniversidad.WebClient.Models.Subjects;
using QCUniversidad.WebClient.Services.Data;
using QCUniversidad.WebClient.Services.Platform;

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
    public async Task<IActionResult> Index(Guid? periodSelected = null, Guid? schoolYearId = null, Guid? courseSelected = null, string? tab = "periodsubjects")
    {
        var workingSchoolYear = (!User.IsAdmin() && schoolYearId is not null) || schoolYearId is null
            ? await _dataProvider.GetCurrentSchoolYear()
            : await _dataProvider.GetSchoolYearAsync(schoolYearId.Value);
        var courses = await _dataProvider.GetCoursesAsync(workingSchoolYear.Id);
        var model = new PlanningIndexModel
        {
            SchoolYearId = workingSchoolYear.Id,
            SchoolYear = workingSchoolYear,
            Periods = await _dataProvider.GetPeriodsAsync(workingSchoolYear.Id),
            PeriodSelected = periodSelected,
            Courses = courses,
            CourseSelected = courseSelected,
            Tab = tab
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetPlanningViewForPeriodAsync(Guid periodId)
    {
        try
        {
            var models = await _dataProvider.GetTeachingPlanItemsAsync(periodId);
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
        var courses = await _dataProvider.GetCoursesAsync(periodModel.SchoolYearId);
        var viewModel = new CreateTeachingPlanItemModel
        {
            PeriodId = periodId,
            Period = periodModel,
            Courses = courses
        };
        return viewModel;
    }

    [HttpGet]
    public async Task<IActionResult> GetSubjectsOptionsForCourseAsync(Guid courseId, Guid periodId)
    {
        if (!await _dataProvider.ExistsCourseAsync(courseId))
        {
            return NotFound();
        }
        if (!await _dataProvider.ExistsPeriodAsync(periodId))
        {
            return NotFound();
        }
        try
        {
            var result = await _dataProvider.GetSubjectsForCourseInPeriodAsync(courseId, periodId);
            return PartialView("_SubjectsOptions", result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
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
                    return RedirectToAction("Index", new { periodSelected = model.PeriodId, tab = "planning" });
                }
            }
            else
            {
                ModelState.AddModelError("GroupsAmount", "Debe de definir al menos un grupo.");
            }
        }
        model.Period = await _dataProvider.GetPeriodAsync(model.PeriodId);
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
        var subjects = await _dataProvider.GetSubjectsForCourseAsync(planningItem.CourseId);
        var course = await _dataProvider.GetCourseAsync(planningItem.CourseId);
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
                    return RedirectToAction("Index", new { periodSelected = model.PeriodId });
                }
            }
            else
            {
                ModelState.AddModelError("GroupsAmount", "Debe de definir al menos un grupo.");
            }
        }
        model.Subjects = await _dataProvider.GetSubjectsForCourseAsync(model.CourseId);
        model.Period = await _dataProvider.GetPeriodAsync(model.PeriodId);
        model.Course = await _dataProvider.GetCourseAsync(model.CourseId);
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

    [HttpGet]
    public async Task<IActionResult> GetPeriodSubjectsViewAsync(Guid periodId, Guid courseId)
    {
        try
        {
            var periodSubjects = await _dataProvider.GetPeriodSubjectsForCourseAsync(periodId, courseId);
            return PartialView("_PeriodSubjectsView", periodSubjects);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetSubjectsForCourseAsync(Guid courseId, Guid periodId)
    {
        try
        {
            var subjects = await _dataProvider.GetSubjectsForCourseNotAssignedInPeriodAsync(courseId, periodId);
            return PartialView("_SubjectSelectOptions", subjects);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> CreatePeriodSubjectAsync([FromBody] CreatePeriodSubjectModel model)
    {
        try
        {
            if (model is null)
            {
                return BadRequest();
            }
            if (!await _dataProvider.ExistsPeriodAsync(model.PeriodId))
            {
                return NotFound("El período seleccionado no existe.");
            }
            if (!await _dataProvider.ExistsCourseAsync(model.CourseId))
            {
                return NotFound("El curso seleccionado no existe.");
            }
            if (!await _dataProvider.ExistsSubjectAsync(model.SubjectId))
            {
                return NotFound("La asignatura seleccionada no existe.");
            }
            var result = await _dataProvider.CreatePeriodSubjectAsync(_mapper.Map<PeriodSubjectModel>(model));
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditPeriodSubjectAsync(PeriodSubjectModel model)
    {
        try
        {
            var result = await _dataProvider.UpdatePeriodSubjectAsync(model);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPeriodSubjectInfoAsync(Guid periodSubjectId)
    {
        if (periodSubjectId == Guid.Empty)
        {
            return BadRequest();
        }
        try
        {
            var result = await _dataProvider.GetPeriodSubjectAsync(periodSubjectId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPeriodInfoAsync(Guid periodId)
    {
        if (periodId == Guid.Empty)
        {
            return BadRequest();
        }
        try
        {
            var result = await _dataProvider.GetPeriodAsync(periodId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetSubjectInfoAsync(Guid subjectId)
    {
        if (subjectId == Guid.Empty)
        {
            return BadRequest();
        }
        try
        {
            var result = await _dataProvider.GetSubjectAsync(subjectId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePeriodSubjectAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest();
        }
        try
        {
            var result = await _dataProvider.DeletePeriodSubjectAsync(id);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}