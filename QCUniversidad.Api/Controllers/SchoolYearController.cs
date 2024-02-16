using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Shared.Dtos.SchoolYear;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SchoolYearController(ISchoolYearsManager dataManager, IMapper mapper) : ControllerBase
{
    private readonly ISchoolYearsManager _dataManager = dataManager;
    private readonly IMapper _mapper = mapper;

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        IList<SchoolYearModel> schoolYears = await _dataManager.GetSchoolYearsAsync(from, to);
        IEnumerable<SchoolYearDto> dtos = schoolYears.Select(_mapper.Map<SchoolYearDto>);
        foreach (SchoolYearDto? dto in dtos)
        {
            dto.CoursesCount = await _dataManager.GetSchoolYearCoursesCountAsync(dto.Id);
        }

        return Ok(dtos);
    }

    [HttpGet("count")]
    public async Task<IActionResult> CountAsync()
    {
        int total = await _dataManager.GetSchoolYearTotalAsync();
        return Ok(total);
    }

    [HttpGet("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id)
    {
        try
        {
            bool result = await _dataManager.ExistSchoolYearAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(NewSchoolYearDto dto)
    {
        if (dto is null)
        {
            return BadRequest("The school year cannot be null.");
        }

        try
        {
            SchoolYearModel model = _mapper.Map<SchoolYearModel>(dto);
            bool result = await _dataManager.CreateSchoolYearAsync(model);
            return result ? Ok() : BadRequest("An error has occured creating the school year.");
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
            SchoolYearModel result = await _dataManager.GetSchoolYearAsync(id);
            SchoolYearDto dto = _mapper.Map<SchoolYearDto>(result);
            dto.CoursesCount = await _dataManager.GetSchoolYearCoursesCountAsync(dto.Id);
            return Ok(dto);
        }
        catch (SchoolYearNotFoundException)
        {
            return NotFound($"The school year with id {id} was not found.");
        }
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentAsync()
    {
        try
        {
            SchoolYearModel result = await _dataManager.GetCurrentSchoolYearAsync();
            SchoolYearDto dto = _mapper.Map<SchoolYearDto>(result);
            dto.CoursesCount = await _dataManager.GetSchoolYearCoursesCountAsync(dto.Id);
            return Ok(dto);
        }
        catch (NotCurrentSchoolYearDefined)
        {
            return NotFound("No defined current school year.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(EditSchoolYearDto dto)
    {
        if (dto is null)
        {
            return BadRequest("The school year cannot be null.");
        }

        bool result = await _dataManager.UpdateSchoolYearAsync(_mapper.Map<SchoolYearModel>(dto));
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteSchoolYearAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You must provide an id.");
        }

        try
        {
            bool result = await _dataManager.DeleteSchoolYearAsync(id);
            return Ok(result);
        }
        catch (SchoolYearNotFoundException)
        {
            return NotFound($"The school year with id '{id}' was not found.");
        }
    }
}
