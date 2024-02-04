using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CareerController(ICareersManager dataManager, IMapper mapper) : ControllerBase
{
    private readonly ICareersManager _dataManager = dataManager;
    private readonly IMapper _mapper = mapper;

    [HttpPut]
    public async Task<IActionResult> CreateCareer(NewCareerDto career)
    {
        if (career is null)
        {
            return BadRequest("You must provide a career.");
        }

        try
        {
            CareerModel model = _mapper.Map<CareerModel>(career);
            bool result = await _dataManager.CreateCareerAsync(model);
            return result ? Ok(result) : Problem("Error while adding career to database.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You must provide a career id.");
        }

        try
        {
            CareerModel career = await _dataManager.GetCareerAsync(id);
            CareerDto dto = _mapper.Map<CareerDto>(career);
            return Ok(dto);
        }
        catch (CareerNotFoundException)
        {
            return NotFound($"The career with id {id} was not found.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetCareers(Guid facultyId)
    {
        if (facultyId == Guid.Empty)
        {
            return BadRequest("You must provide a faculty id.");
        }

        try
        {
            IList<CareerModel> careers = await _dataManager.GetCareersAsync(facultyId);
            List<CareerDto> dtos = careers.Select(_mapper.Map<CareerDto>).ToList();
            return Ok(dtos);
        }
        catch (FacultyNotFoundException)
        {
            return NotFound($"The faculty with the id {facultyId} was not found.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("listfordepartment")]
    public async Task<IActionResult> GetCareersForDepartmentAsync(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest("You must provide a department id.");
        }

        try
        {
            IList<CareerModel> careers = await _dataManager.GetCareersForDepartmentAsync(departmentId);
            List<CareerDto> dtos = careers.Select(_mapper.Map<CareerDto>).ToList();
            return Ok(dtos);
        }
        catch (DepartmentNotFoundException)
        {
            return NotFound($"The department with the id {departmentId} was not found.");
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
            bool result = await _dataManager.ExistsCareerAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("listall")]
    public async Task<IActionResult> GetCareers(int from = 0, int to = 0)
    {
        try
        {
            IList<CareerModel> careers = await _dataManager.GetCareersAsync(from, to);
            List<CareerDto> dtos = careers.Select(_mapper.Map<CareerDto>).ToList();
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("countall")]
    public async Task<IActionResult> GetCount()
    {
        try
        {
            int count = await _dataManager.GetCareersCountAsync();
            return Ok(count);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> UpdateCareer(EditCareerDto career)
    {
        if (career is null)
        {
            return BadRequest("You must provide a faculty id.");
        }

        try
        {
            CareerModel model = _mapper.Map<CareerModel>(career);
            bool result = await _dataManager.UpdateCareerAsync(model);
            return result ? Ok(result) : Problem("Error while adding career to database.");
        }
        catch (FacultyNotFoundException)
        {
            return NotFound($"The career with id {career.Id} was not found in database.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCareer(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You must provide a career id.");
        }

        try
        {
            bool result = await _dataManager.DeleteCareerAsync(id);
            return result ? Ok(result) : Problem("Error while deleting department from database.");
        }
        catch (CareerNotFoundException)
        {
            return NotFound($"The career with id {id} was not found in database.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}