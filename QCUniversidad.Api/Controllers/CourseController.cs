using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseController(ICoursesManager courseManager,
                              ISchoolYearsManager schoolYearsManager,
                              IFacultiesManager facultiesManager,
                              ICareersManager careersManager,
                              IPeriodsManager periodsManager,
                              IMapper mapper) : ControllerBase
{
    private readonly ICoursesManager _courseManager = courseManager;
    private readonly ISchoolYearsManager _schoolYearsManager = schoolYearsManager;
    private readonly IFacultiesManager _facultiesManager = facultiesManager;
    private readonly ICareersManager _careersManager = careersManager;
    private readonly IPeriodsManager _periodsManager = periodsManager;
    private readonly IMapper _mapper = mapper;

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        IList<CourseModel> courses = await _courseManager.GetCoursesAsync(from, to);
        IEnumerable<CourseDto> dtos = courses.Select(_mapper.Map<CourseDto>);
        return Ok(dtos);
    }

    [HttpGet]
    [Route("listbyschoolyear")]
    public async Task<IActionResult> GetListBySchoolYearAsync(Guid schoolYearId)
    {
        try
        {
            if (!await _schoolYearsManager.ExistSchoolYearAsync(schoolYearId))
            {
                return NotFound();
            }

            IList<CourseModel> result = await _courseManager.GetCoursesAsync(schoolYearId);
            IOrderedEnumerable<CourseDto> dtos = result.Select(_mapper.Map<CourseDto>).OrderBy(dto => dto.CareerId).ThenBy(dto => dto.CareerYear);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("listbyschoolyearandfaculty")]
    public async Task<IActionResult> GetListBySchoolYearAndFacultyAsync(Guid schoolYearId, Guid facultyId)
    {
        try
        {
            if (!await _schoolYearsManager.ExistSchoolYearAsync(schoolYearId))
            {
                return NotFound();
            }

            if (!await _facultiesManager.ExistFacultyAsync(facultyId))
            {
                return NotFound();
            }

            IList<CourseModel> result = await _courseManager.GetCoursesAsync(schoolYearId, facultyId);
            IOrderedEnumerable<CourseDto> dtos = result.Select(_mapper.Map<CourseDto>).OrderBy(dto => dto.CareerId).ThenBy(dto => dto.CareerYear);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("listbycareerschoolyearandfaculty")]
    public async Task<IActionResult> GetListByCareerSchoolYearAndFacultyAsync(Guid careerId, Guid schoolYearId, Guid facultyId)
    {
        try
        {
            if (!await _careersManager.ExistsCareerAsync(careerId))
            {
                return NotFound();
            }

            if (!await _schoolYearsManager.ExistSchoolYearAsync(schoolYearId))
            {
                return NotFound();
            }

            if (!await _facultiesManager.ExistFacultyAsync(facultyId))
            {
                return NotFound();
            }

            IList<CourseModel> result = await _courseManager.GetCoursesAsync(careerId, schoolYearId, facultyId);
            IOrderedEnumerable<CourseDto> dtos = result.Select(_mapper.Map<CourseDto>).OrderBy(dto => dto.CareerId).ThenBy(dto => dto.CareerYear);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCount()
    {
        try
        {
            int count = await _courseManager.GetCoursesCountAsync();
            return Ok(count);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("periodscount")]
    public async Task<IActionResult> GetPeriodsCount(Guid schoolYearId)
    {
        try
        {
            int count = await _periodsManager.GetSchoolYearPeriodsCountAsync(schoolYearId);
            return Ok(count);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id)
    {
        try
        {
            bool result = await _courseManager.ExistsCourseAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("existsbycareeryearandmodality")]
    public async Task<IActionResult> ExistsByCareerYearAndModality(Guid careerId, int careerYear, int modality)
    {
        if (careerId == Guid.Empty && careerYear < 0 && modality < 0)
        {
            return BadRequest("The parameters should not be null.");
        }

        bool result = await _courseManager.CheckCourseExistenceByCareerYearAndModality(careerId, careerYear, (TeachingModality)modality);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(NewCourseDto course)
    {
        if (course is null)
        {
            return BadRequest("The discipline cannot be null.");
        }

        try
        {
            CourseModel model = _mapper.Map<CourseModel>(course);
            bool result = await _courseManager.CreateCourseAsync(model);
            return result ? Ok(model.Id) : Problem("An error has occured creating the discipline.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You must provide an id.");
        }

        try
        {
            CourseModel result = await _courseManager.GetCourseAsync(id);
            CourseDto dto = _mapper.Map<CourseDto>(result);
            return Ok(dto);
        }
        catch (CourseNotFoundException)
        {
            return NotFound($"The school year with id {id} was not found.");
        }
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(EditCourseDto course)
    {
        if (course is not null)
        {
            bool result = await _courseManager.UpdateCourseAsync(_mapper.Map<CourseModel>(course));
            return Ok(result);
        }

        return BadRequest("The school year cannot be null.");
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You must provide an id.");
        }

        try
        {
            bool result = await _courseManager.DeleteCourseAsync(id);
            return Ok(result);
        }
        catch (CourseNotFoundException)
        {
            return NotFound($"The school year with id '{id}' was not found.");
        }
    }

    [HttpGet]
    [Route("listfordepartment")]
    public async Task<IActionResult> GetCoursesForDepartment(Guid departmentId, Guid? schoolYearId = null)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest("You must provide an id.");
        }

        try
        {
            IList<CourseModel> result = await _courseManager.GetCoursesForDepartmentAsync(departmentId, schoolYearId);
            IEnumerable<CourseDto> dtos = result.Select(_mapper.Map<CourseDto>);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("periodplanninginfo")]
    public async Task<IActionResult> GetPeriodPlanningInfoAsync(Guid id, Guid periodId)
    {
        try
        {
            CourseModel course = await _courseManager.GetCourseAsync(id);
            PeriodModel period = await _periodsManager.GetPeriodAsync(periodId);
            double totalPlanned = await _courseManager.GetTotalHoursInPeriodForCourseAsync(id, periodId);
            double realPlanned = await _courseManager.GetRealHoursPlannedInPeriodForCourseAsync(id, periodId);
            CoursePeriodPlanningInfoDto dto = new()
            {
                PeriodId = periodId,
                Period = _mapper.Map<SimplePeriodDto>(period),
                CourseId = id,
                Course = _mapper.Map<SimpleCourseDto>(course),
                RealHoursPlanned = realPlanned,
                TotalHoursPlanned = totalPlanned
            };
            return Ok(dto);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (CourseNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (PeriodNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}