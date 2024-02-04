using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Subject;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubjectController(ISubjectsManager subjectsManager,
                               ICoursesManager coursesManager,
                               IPeriodsManager periodsManager,
                               IMapper mapper) : ControllerBase
{
    private readonly ISubjectsManager _subjectsManager = subjectsManager;
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly IPeriodsManager _periodsManager = periodsManager;
    private readonly IMapper _mapper = mapper;

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        IList<SubjectModel> subjects = await _subjectsManager.GetSubjectsAsync(from, to);
        IEnumerable<SubjectDto> dtos = subjects.Select(_mapper.Map<SubjectDto>);
        return Ok(dtos);
    }

    [HttpGet]
    [Route("listfordiscipline")]
    public async Task<IActionResult> GetListForDisciplineAsync(Guid disciplineId)
    {
        try
        {
            IList<SubjectModel> subjects = await _subjectsManager.GetSubjectsForDisciplineAsync(disciplineId);
            IEnumerable<SubjectDto> dtos = subjects.Select(_mapper.Map<SubjectDto>);
            return Ok(dtos);
        }
        catch (ArgumentNullException)
        {
            return BadRequest();
        }
        catch (DisciplineNotFoundException)
        {
            return NotFound("Discipline not found.");
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message);
        }
    }

    [HttpGet]
    [Route("getforcourse")]
    public async Task<IActionResult> GetListForCourse(Guid courseId)
    {
        try
        {
            if (!await _coursesManager.ExistsCourseAsync(courseId))
            {
                return BadRequest("The course do not exists.");
            }

            IList<SubjectModel> result = await _subjectsManager.GetSubjectsForCourseAsync(courseId);
            IEnumerable<SubjectDto> dtos = result.Select(_mapper.Map<SubjectDto>);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("getforcourseinperiod")]
    public async Task<IActionResult> GetListForCourseInPeriodAsync(Guid courseId, Guid periodId)
    {
        try
        {
            IList<SubjectModel> subjects = await _subjectsManager.GetSubjectsForCourseInPeriodAsync(courseId, periodId);
            IEnumerable<SubjectDto> dtos = subjects.Select(_mapper.Map<SubjectDto>);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("getforcoursenotassignedtoperiod")]
    public async Task<IActionResult> GetListForCourseNotAssignedInPeriod(Guid courseId, Guid periodId)
    {
        try
        {
            if (!await _coursesManager.ExistsCourseAsync(courseId))
            {
                return NotFound("The course do not exists.");
            }

            if (!await _periodsManager.ExistsPeriodAsync(periodId))
            {
                return NotFound("The period do not exists.");
            }

            IList<SubjectModel> result = await _subjectsManager.GetSubjectsForCourseNotAssignedInPeriodAsync(courseId, periodId);
            IEnumerable<SubjectDto> dtos = result.Select(_mapper.Map<SubjectDto>);
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
            int count = await _subjectsManager.GetSubjectsCountAsync();
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
            bool result = await _subjectsManager.ExistsSubjectAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("existsbyname")]
    public async Task<IActionResult> ExistsByNameAsync(string name)
    {
        try
        {
            bool result = await _subjectsManager.ExistsSubjectAsync(name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(NewSubjectDto subjectDto)
    {
        if (subjectDto is null)
        {
            return BadRequest("The subject cannot be null.");
        }

        bool result = await _subjectsManager.CreateSubjectAsync(_mapper.Map<SubjectModel>(subjectDto));
        return result ? Ok() : Problem("An error has occured creating the subject.");
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
            SubjectModel result = await _subjectsManager.GetSubjectAsync(id);
            SubjectDto dto = _mapper.Map<SubjectDto>(result);
            return Ok(dto);
        }
        catch (SubjectNotFoundException)
        {
            return NotFound($"The subject with id {id} was not found.");
        }
    }

    [HttpGet]
    [Route("byname")]
    public async Task<IActionResult> GetByIdAsync(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("You must provide an id.");
        }

        try
        {
            SubjectModel result = await _subjectsManager.GetSubjectAsync(name);
            SubjectDto dto = _mapper.Map<SubjectDto>(result);
            return Ok(dto);
        }
        catch (SubjectNotFoundException)
        {
            return NotFound($"The subject with name {name} was not found.");
        }
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(EditSubjectDto subject)
    {
        if (subject is null)
        {
            return BadRequest("The subject cannot be null.");
        }

        SubjectModel model = _mapper.Map<SubjectModel>(subject);
        bool result = await _subjectsManager.UpdateSubjectAsync(model);
        return Ok(result);
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
            bool result = await _subjectsManager.DeleteSubjectAsync(id);
            return Ok(result);
        }
        catch (SubjectNotFoundException)
        {
            return NotFound($"The subject with id '{id}' was not found.");
        }
    }

    [HttpGet]
    [Route("periodsubjects")]
    public async Task<IActionResult> GetPeriodSubjectsAsync(Guid periodId, Guid courseId)
    {
        try
        {
            IList<PeriodSubjectModel> periodSubjects = await _subjectsManager.GetPeriodSubjectsForCourseAsync(periodId, courseId);
            IEnumerable<SimplePeriodSubjectDto> dtos = periodSubjects.Select(_mapper.Map<SimplePeriodSubjectDto>);
            return Ok(dtos);
        }
        catch (PeriodNotFoundException)
        {
            return NotFound();
        }
        catch (CourseNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    [Route("periodsubject")]
    public async Task<IActionResult> CreatePeriodSubjectAsync(NewPeriodSubjectDto dto)
    {
        try
        {
            PeriodSubjectModel model = _mapper.Map<PeriodSubjectModel>(dto);
            bool result = await _subjectsManager.CreatePeriodSubjectAsync(model);
            return result ? Ok(result) : Problem();
        }
        catch (PeriodNotFoundException)
        {
            return NotFound("El período no existe.");
        }
        catch (CourseNotFoundException)
        {
            return NotFound("El curso no existe.");
        }
        catch (SubjectNotFoundException)
        {
            return NotFound("La asignatura no existe.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("periodsubject")]
    public async Task<IActionResult> GetPeriodSubjectAsync(Guid id)
    {
        try
        {
            PeriodSubjectModel result = await _subjectsManager.GetPeriodSubjectAsync(id);
            PeriodSubjectDto dto = _mapper.Map<PeriodSubjectDto>(result);
            return Ok(dto);
        }
        catch (PeriodSubjectNotFoundException)
        {
            return NotFound("The period subject was not found.");
        }
    }

    [HttpPost]
    [Route("updateperiodsubject")]
    public async Task<IActionResult> UpdatePeriodSubjectAsync(EditPeriodSubjectDto periodSubject)
    {
        try
        {
            PeriodSubjectModel model = _mapper.Map<PeriodSubjectModel>(periodSubject);
            bool result = await _subjectsManager.UpdatePeriodSubjectAsync(model);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete]
    [Route("deleteperiodsubject")]
    public async Task<IActionResult> DeletePeriodSubjectAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("The id of the period subject should not be empty.");
        }

        try
        {
            bool result = await _subjectsManager.DeletePeriodSubjectAsync(id);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}