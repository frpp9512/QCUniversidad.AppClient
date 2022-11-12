using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Subject;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SubjectController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly IMapper _mapper;

    public SubjectController(IDataManager dataManager, IMapper mapper)
    {
        _dataManager = dataManager;
        _mapper = mapper;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        var subjects = await _dataManager.GetSubjectsAsync(from, to);
        var dtos = subjects.Select(s => _mapper.Map<SubjectDto>(s));
        return Ok(dtos);
    }

    [HttpGet]
    [Route("getforcourse")]
    public async Task<IActionResult> GetListForCourse(Guid courseId)
    {
        try
        {
            if (!await _dataManager.ExistsCourseAsync(courseId))
            {
                return BadRequest("The course do not exists.");
            }
            var result = await _dataManager.GetSubjectsForCourseAsync(courseId);
            var dtos = result.Select(r => _mapper.Map<SubjectDto>(r));
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
            var subjects = await _dataManager.GetSubjectsForCourseInPeriodAsync(courseId, periodId);
            var dtos = subjects.Select(s => _mapper.Map<SubjectDto>(s));
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
            if (!await _dataManager.ExistsCourseAsync(courseId))
            {
                return NotFound("The course do not exists.");
            }
            if (!await _dataManager.ExistsPeriodAsync(periodId))
            {
                return NotFound("The period do not exists.");
            }
            var result = await _dataManager.GetSubjectsForCourseNotAssignedInPeriodAsync(courseId, periodId);
            var dtos = result.Select(r => _mapper.Map<SubjectDto>(r));
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
            var count = await _dataManager.GetSubjectsCountAsync();
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
            var result = await _dataManager.ExistsSubjectAsync(id);
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
            var result = await _dataManager.ExistsSubjectAsync(name);
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
        if (subjectDto is not null)
        {
            var result = await _dataManager.CreateSubjectAsync(_mapper.Map<SubjectModel>(subjectDto));
            return result ? Ok() : Problem("An error has occured creating the subject.");
        }
        return BadRequest("The subject cannot be null.");
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
            var result = await _dataManager.GetSubjectAsync(id);
            var dto = _mapper.Map<SubjectDto>(result);
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
            var result = await _dataManager.GetSubjectAsync(name);
            var dto = _mapper.Map<SubjectDto>(result);
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
        if (subject is not null)
        {
            var model = _mapper.Map<SubjectModel>(subject);
            var result = await _dataManager.UpdateSubjectAsync(model);
            return Ok(result);
        }
        return BadRequest("The subject cannot be null.");
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
            var result = await _dataManager.DeleteSubjectAsync(id);
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
            var periodSubjects = await _dataManager.GetPeriodSubjectsForCourseAsync(periodId, courseId);
            var dtos = periodSubjects.Select(ps => _mapper.Map<SimplePeriodSubjectDto>(ps));
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
            var model = _mapper.Map<PeriodSubjectModel>(dto);
            var result = await _dataManager.CreatePeriodSubjectAsync(model);
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
            var result = await _dataManager.GetPeriodSubjectAsync(id);
            var dto = _mapper.Map<PeriodSubjectDto>(result);
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
            var model = _mapper.Map<PeriodSubjectModel>(periodSubject);
            var result = await _dataManager.UpdatePeriodSubjectAsync(model);
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
            var result = await _dataManager.DeletePeriodSubjectAsync(id);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}