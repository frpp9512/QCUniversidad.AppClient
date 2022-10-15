using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.Api.Shared.Dtos.SchoolYear;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SchoolYearController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly IMapper _mapper;

    public SchoolYearController(IDataManager dataManager, IMapper mapper)
    {
        _dataManager = dataManager;
        _mapper = mapper;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        var schoolYears = await _dataManager.GetSchoolYearsAsync(from, to);
        var dtos = schoolYears.Select(d => _mapper.Map<SchoolYearDto>(d));
        return Ok(dtos);
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCount()
    {
        try
        {
            var count = await _dataManager.GetSchoolYearsCountAsync();
            return Ok(count);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("periodscount")]
    public async Task<IActionResult> GetPeriodsCount(Guid id)
    {
        try
        {
            var count = await _dataManager.GetSchoolYearPeriodsCountAsync(id);
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
            var result = await _dataManager.ExistsSchoolYearAsync(id);
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
        if (careerId != Guid.Empty || careerYear >= 0 || modality >= 0)
        {
            var result = await _dataManager.CheckSchoolYearExistenceByCareerYearAndModality(careerId, careerYear, (TeachingModality)modality);
            return Ok(result);
        }
        return BadRequest("The parameters should not be null.");
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(NewSchoolYearDto schoolYear)
    {
        if (schoolYear is not null)
        {
            var model = _mapper.Map<SchoolYearModel>(schoolYear);
            var result = await _dataManager.CreateSchoolYearAsync(model);
            return result ? Ok(model.Id) : Problem("An error has occured creating the discipline.");
        }
        return BadRequest("The discipline cannot be null.");
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
            var result = await _dataManager.GetSchoolYearAsync(id);
            var dto = _mapper.Map<SchoolYearDto>(result);
            return Ok(dto);
        }
        catch (SchoolYearNotFoundException)
        {
            return NotFound($"The school year with id {id} was not found.");
        }
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(EditSchoolYearDto schoolYear)
    {
        if (schoolYear is not null)
        {
            var result = await _dataManager.UpdateSchoolYearAsync(_mapper.Map<SchoolYearModel>(schoolYear));
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
            var result = await _dataManager.DeleteSchoolYearAsync(id);
            return Ok(result);
        }
        catch (SchoolYearNotFoundException)
        {
            return NotFound($"The school year with id '{id}' was not found.");
        }
    }

    [HttpGet]
    [Route("listfordepartment")]
    public async Task<IActionResult> GetSchoolYearsForDepartment(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest("You must provide an id.");
        }
        try
        {
            var result = await _dataManager.GetSchoolYearsForDepartment(departmentId);
            var dtos = result.Select(i => _mapper.Map<SchoolYearDto>(i));
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}