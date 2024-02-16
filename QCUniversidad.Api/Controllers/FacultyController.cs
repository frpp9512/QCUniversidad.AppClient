using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FacultyController(IFacultiesManager facultiesManager,
                               IMapper mapper) : ControllerBase
{
    private readonly IFacultiesManager _facultiesManager = facultiesManager;
    private readonly IMapper _mapper = mapper;

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        IList<FacultyModel> faculties = await _facultiesManager.GetFacultiesAsync(from, to);
        IEnumerable<FacultyDto> dtos = faculties.Select(f => GetFacultyDto(f).GetAwaiter().GetResult());
        return Ok(dtos);
    }

    [HttpGet("count")]
    public async Task<IActionResult> CountAsync()
    {
        int total = await _facultiesManager.GetFacultiesTotalAsync();
        return Ok(total);
    }

    [HttpGet("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id)
    {
        try
        {
            bool result = await _facultiesManager.ExistFacultyAsync(id);
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
            bool result = await _facultiesManager.CreateFacultyAsync(_mapper.Map<FacultyModel>(facultyDto));
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
            FacultyModel result = await _facultiesManager.GetFacultyAsync(id);
            FacultyDto dto = await GetFacultyDto(result);
            return Ok(dto);
        }
        catch (FacultyNotFoundException)
        {
            return NotFound($"The faculty with id {id} was not found.");
        }
    }

    private async Task<FacultyDto> GetFacultyDto(FacultyModel model)
    {
        FacultyDto dto = _mapper.Map<FacultyDto>(model);
        dto.CareersCount = await _facultiesManager.GetFacultyCareerCountAsync(model.Id);
        dto.DepartmentCount = await _facultiesManager.GetFacultyDepartmentCountAsync(model.Id);
        return dto;
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(FacultyDto faculty)
    {
        if (faculty is null)
        {
            return BadRequest("The faculty cannot be null.");
        }

        bool result = await _facultiesManager.UpdateFacultyAsync(_mapper.Map<FacultyModel>(faculty));
        return Ok(result);
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
            bool result = await _facultiesManager.DeleteFacultyAsync(id);
            return Ok(result);
        }
        catch (FacultyNotFoundException)
        {
            return NotFound($"The faculty with id '{id}' was not found.");
        }
    }
}