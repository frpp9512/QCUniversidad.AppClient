using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.SchoolYear;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
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
        var dtos = schoolYears.Select(sy => _mapper.Map<SchoolYearDto>(sy));
        foreach (var dto in dtos)
        {
            dto.CoursesCount = await _dataManager.GetSchoolYearCoursesCountAsync(dto.Id);
        }
        return Ok(dtos);
    }

    [HttpGet("count")]
    public async Task<IActionResult> CountAsync()
    {
        var total = await _dataManager.GetSchoolYearTotalAsync();
        return Ok(total);
    }

    [HttpGet("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id)
    {
        try
        {
            var result = await _dataManager.ExistSchoolYearAsync(id);
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
        if (dto is not null)
        {
            try
            {
                var model = _mapper.Map<SchoolYearModel>(dto);
                var result = await _dataManager.CreateSchoolYearAsync(model);
                return result ? Ok() : BadRequest("An error has occured creating the school year.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        return BadRequest("The school year cannot be null.");
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
            var result = await _dataManager.GetCurrentSchoolYearAsync();
            var dto = _mapper.Map<SchoolYearDto>(result);
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
        if (dto is not null)
        {
            var result = await _dataManager.UpdateSchoolYearAsync(_mapper.Map<SchoolYearModel>(dto));
            return Ok(result);
        }
        return BadRequest("The school year cannot be null.");
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
            var result = await _dataManager.DeleteSchoolYearAsync(id);
            return Ok(result);
        }
        catch (SchoolYearNotFoundException)
        {
            return NotFound($"The school year with id '{id}' was not found.");
        }
    }
}
