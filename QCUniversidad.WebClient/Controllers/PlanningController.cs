using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.Course;
using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Models.Subjects;
using QCUniversidad.WebClient.Services.Contracts;
using QCUniversidad.WebClient.Services.Platform;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Planner")]
public class PlanningController(IFacultiesDataProvider facultiesDataProvider,
                                ISchoolYearDataProvider schoolYearDataProvider,
                                ICareersDataProvider careersDataProvider,
                                IPeriodsDataProvider periodsDataProvider,
                                ICoursesDataProvider coursesDataProvider,
                                IPlanningDataProvider planningDataProvider,
                                ISubjectsDataProvider subjectsDataProvider,
                                IMapper mapper,
                                IOptions<NavigationSettings> navOptions,
                                ILogger<PlanningController> logger) : Controller
{
    private readonly IFacultiesDataProvider _facultiesDataProvider = facultiesDataProvider;
    private readonly ISchoolYearDataProvider _schoolYearDataProvider = schoolYearDataProvider;
    private readonly ICareersDataProvider _careersDataProvider = careersDataProvider;
    private readonly IPeriodsDataProvider _periodsDataProvider = periodsDataProvider;
    private readonly ICoursesDataProvider _coursesDataProvider = coursesDataProvider;
    private readonly IPlanningDataProvider _planningDataProvider = planningDataProvider;
    private readonly ISubjectsDataProvider _subjectsDataProvider = subjectsDataProvider;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IActionResult> Index(Guid? facultyId = null, Guid? periodSelected = null, Guid? schoolYearId = null, Guid? courseSelected = null, Guid? careerSelected = null, string? tab = "planning")
    {
        if (facultyId is null && User.IsAdmin())
        {
            return RedirectToAction("SelectFaculty");
        }

        try
        {
            Models.Faculties.FacultyModel faculty = await _facultiesDataProvider.GetFacultyAsync((User.IsAdmin() && facultyId is not null) ? facultyId.Value : User.GetFacultyId());
            Models.SchoolYears.SchoolYearModel workingSchoolYear = (!User.IsAdmin() && schoolYearId is not null) || schoolYearId is null
                                        ? await _schoolYearDataProvider.GetCurrentSchoolYear()
                                        : await _schoolYearDataProvider.GetSchoolYearAsync(schoolYearId.Value);
            IList<Models.Careers.CareerModel> careers = await _careersDataProvider.GetCareersAsync(faculty.Id);
            PlanningIndexModel model = new()
            {
                Faculty = faculty,
                SchoolYearId = workingSchoolYear.Id,
                SchoolYear = workingSchoolYear,
                Careers = careers,
                Periods = await _periodsDataProvider.GetPeriodsAsync(workingSchoolYear.Id),
                PeriodSelected = periodSelected,
                CourseSelected = courseSelected,
                CareerSelected = careerSelected,
                Tab = tab ?? "planning"
            };
            return View(model);
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Authorize("Admin")]
    public async Task<IActionResult> SelectFacultyAsync(string returnUrl = "Index")
    {
        IList<Models.Faculties.FacultyModel> faculties = await _facultiesDataProvider.GetFacultiesAsync();
        if (faculties.Count == 1)
        {
            return RedirectToAction("Index", new { facultyId = faculties.First().Id });
        }

        ViewData["returnUrl"] = returnUrl ?? "Index";
        return View(faculties);
    }

    [HttpGet]
    public async Task<IActionResult> GetCoursesOptionSelectorsAsync(Guid careerId, Guid? facultyId = null)
    {
        Models.SchoolYears.SchoolYearModel schoolYear = await _schoolYearDataProvider.GetCurrentSchoolYear();
        IList<CourseModel> courses = await _coursesDataProvider.GetCoursesAsync(careerId, schoolYear.Id, (User.IsAdmin() && facultyId is not null) ? facultyId.Value : User.GetFacultyId());
        return PartialView("_CoursesOptionSelectors", courses);
    }

    [HttpGet]
    public async Task<IActionResult> GetPlanningViewForPeriodAsync(Guid periodId, Guid? courseId = null)
    {
        try
        {
            IList<TeachingPlanItemModel> models = await _planningDataProvider.GetTeachingPlanItemsAsync(periodId, courseId);
            return PartialView("_PlanningListView", models);
        }
        catch (Exception)
        {
            return Problem();
        }
    }

    [HttpGet]
    public async Task<IActionResult> CreatePlanningItemAsync(Guid periodId, Guid? courseId, string returnTo = "Index")
    {
        try
        {
            if (periodId == Guid.Empty)
            {
                return RedirectToAction("Error", "Home");
            }

            if (!await _periodsDataProvider.ExistsPeriodAsync(periodId))
            {
                return RedirectToAction("Error", "Home");
            }

            CreateTeachingPlanItemModel viewModel = await GetCreateViewModel(periodId, courseId);
            viewModel.ReturnTo = returnTo;
            return View(viewModel);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    private async Task<CreateTeachingPlanItemModel> GetCreateViewModel(Guid periodId, Guid? courseId)
    {
        Models.Periods.PeriodModel periodModel = await _periodsDataProvider.GetPeriodAsync(periodId);
        IList<CourseModel> courses = await _coursesDataProvider.GetCoursesAsync(periodModel.SchoolYearId);
        Guid? careerId = courses.FirstOrDefault(c => c.Id == courseId)?.CareerId;
        CourseModel? course = null;
        if (courseId is not null)
        {
            course = await _coursesDataProvider.GetCourseAsync(courseId.Value);
        }

        CreateTeachingPlanItemModel viewModel = new()
        {
            PeriodId = periodId,
            CourseId = courseId ?? Guid.Empty,
            Period = periodModel,
            Courses = courses,
            CareerId = careerId,
            Course = course
        };
        return viewModel;
    }

    [HttpGet]
    public async Task<IActionResult> GetSubjectsOptionsForCourseAsync(Guid courseId, Guid periodId)
    {
        if (!await _coursesDataProvider.ExistsCourseAsync(courseId))
        {
            return NotFound();
        }

        if (!await _periodsDataProvider.ExistsPeriodAsync(periodId))
        {
            return NotFound();
        }

        try
        {
            IList<SubjectModel> result = await _subjectsDataProvider.GetSubjectsForCourseInPeriodAsync(courseId, periodId);
            return PartialView("_SubjectsOptions", result.OrderBy(s => s.Name).ToList());
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
                TeachingPlanItemModel planItem = _mapper.Map<TeachingPlanItemModel>(model);
                bool result = await _planningDataProvider.CreateTeachingPlanItemAsync(planItem);
                if (result)
                {
                    TempData["planItem-created"] = true;
                    return model.ReturnTo is not null
                        ? Redirect(model.ReturnTo)
                        : RedirectToAction("Index", new
                        {
                            periodSelected = model.PeriodId,
                            courseSelected = model.CourseId,
                            careerSelected = model.CareerId,
                            tab = "planning"
                        });
                }
            }
            else
            {
                ModelState.AddModelError("GroupsAmount", "Debe de definir al menos un grupo.");
            }
        }

        model.Period = await _periodsDataProvider.GetPeriodAsync(model.PeriodId);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditPlanningItemAsync(Guid id)
    {
        if (id == Guid.Empty || !await _planningDataProvider.ExistsTeachingPlanItemAsync(id))
        {
            return RedirectToAction("Error", "Home");
        }

        EditTeachingPlanItemModel editModel = await GetEditViewModel(id);
        return View(editModel);
    }

    private async Task<EditTeachingPlanItemModel> GetEditViewModel(Guid id)
    {
        TeachingPlanItemModel planningItem = await _planningDataProvider.GetTeachingPlanItemAsync(id);
        Models.Periods.PeriodModel periodModel = await _periodsDataProvider.GetPeriodAsync(planningItem.PeriodId);
        IList<SubjectModel> subjects = await _subjectsDataProvider.GetSubjectsForCourseAsync(planningItem.CourseId);
        CourseModel course = await _coursesDataProvider.GetCourseAsync(planningItem.CourseId);
        EditTeachingPlanItemModel viewModel = _mapper.Map<EditTeachingPlanItemModel>(planningItem);
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
                TeachingPlanItemModel planItem = _mapper.Map<TeachingPlanItemModel>(model);
                bool result = await _planningDataProvider.UpdateTeachingPlanItemAsync(planItem);
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

        model.Subjects = await _subjectsDataProvider.GetSubjectsForCourseAsync(model.CourseId);
        model.Period = await _periodsDataProvider.GetPeriodAsync(model.PeriodId);
        model.Course = await _coursesDataProvider.GetCourseAsync(model.CourseId);
        return View(model);
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePlanningItemAsync(Guid id)
    {
        if (id != Guid.Empty && await _planningDataProvider.ExistsTeachingPlanItemAsync(id))
        {
            bool result = await _planningDataProvider.DeleteTeachingPlanItemAsync(id);
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
            IList<PeriodSubjectModel> periodSubjects = await _subjectsDataProvider.GetPeriodSubjectsForCourseAsync(periodId, courseId);
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
            IList<SubjectModel> subjects = await _subjectsDataProvider.GetSubjectsForCourseNotAssignedInPeriodAsync(courseId, periodId);
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

            if (!await _periodsDataProvider.ExistsPeriodAsync(model.PeriodId))
            {
                return NotFound("El período seleccionado no existe.");
            }

            if (!await _coursesDataProvider.ExistsCourseAsync(model.CourseId))
            {
                return NotFound("El curso seleccionado no existe.");
            }

            if (!await _subjectsDataProvider.ExistsSubjectAsync(model.SubjectId))
            {
                return NotFound("La asignatura seleccionada no existe.");
            }

            bool result = await _subjectsDataProvider.CreatePeriodSubjectAsync(_mapper.Map<PeriodSubjectModel>(model));
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
            bool result = await _subjectsDataProvider.UpdatePeriodSubjectAsync(model);
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
            PeriodSubjectModel result = await _subjectsDataProvider.GetPeriodSubjectAsync(periodSubjectId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    public async Task<IActionResult> GetPeriodPlanningInfoAsync(Guid periodId, Guid courseId)
    {
        if (periodId == Guid.Empty)
        {
            return BadRequest();
        }

        try
        {
            CoursePeriodPlanningInfoModel model = await _careersDataProvider.GetCoursePeriodPlanningInfoAsync(courseId, periodId);
            return PartialView("_PeriodPlanningInfo", model);
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
            Models.Periods.PeriodModel result = await _periodsDataProvider.GetPeriodAsync(periodId);
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
            SubjectModel result = await _subjectsDataProvider.GetSubjectAsync(subjectId);
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
            bool result = await _subjectsDataProvider.DeletePeriodSubjectAsync(id);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}