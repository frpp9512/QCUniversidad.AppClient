using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Migrations;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    [Route("getforschoolyear")]
    public async Task<IActionResult> GetListForSchoolYear(Guid schoolYearId)
    {
        try
        {
            if (!await _dataManager.ExistsSchoolYearAsync(schoolYearId))
            {
                return BadRequest("The school year do not exists.");
            }
            var result = await _dataManager.GetSubjectsForSchoolYearAsync(schoolYearId);
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
}