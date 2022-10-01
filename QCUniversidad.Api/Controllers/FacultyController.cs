using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Faculty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class FacultyController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly IMapper _mapper;

    public FacultyController(IDataManager dataManager, IMapper mapper)
    {
        _dataManager = dataManager;
        _mapper = mapper;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        var faculties = await _dataManager.GetFacultiesAsync(from, to);
        var dtos = faculties.Select(f => GetFacultyDto(f).GetAwaiter().GetResult());
        return Ok(dtos);
    }

    [HttpGet("count")]
    public async Task<IActionResult> CountAsync()
    {
        var total = await _dataManager.GetFacultiesTotalAsync();
        return Ok(total);
    }

    [HttpGet("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id)
    {
        try
        {
            var result = await _dataManager.ExistFacultyAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(FacultyDto facultyDto)
    {
        if (facultyDto is not null)
        {
            var result = await _dataManager.CreateFacultyAsync(_mapper.Map<FacultyModel>(facultyDto));
            return result ? Ok() : BadRequest("An error has occured creating the faculty.");
        }
        return BadRequest("The faculty cannot be null.");
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
            var result = await _dataManager.GetFacultyAsync(id);
            var dto = await GetFacultyDto(result);
            return Ok(dto);
        }
        catch (FacultyNotFoundException)
        {
            return NotFound($"The faculty with id {id} was not found.");
        }
    }

    private async Task<FacultyDto> GetFacultyDto(FacultyModel model)
    {
        var dto = _mapper.Map<FacultyDto>(model);
        dto.CareersCount = await _dataManager.GetFacultyCareerCountAsync(model.Id);
        dto.DepartmentCount = await _dataManager.GetFacultyDepartmentCountAsync(model.Id);
        return dto;
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(FacultyDto faculty)
    {
        if (faculty is not null)
        {
            var result = await _dataManager.UpdateFacultyAsync(_mapper.Map<FacultyModel>(faculty));
            return Ok(result);
        }
        return BadRequest("The faculty cannot be null.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFaculty(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You must provide an id.");
        }
        try
        {
            var result = await _dataManager.DeleteFacultyAsync(id);
            return Ok(result);
        }
        catch (FacultyNotFoundException)
        {
            return NotFound($"The faculty with id '{id}' was not found.");
        }
    }
}