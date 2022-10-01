using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Career;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CareerController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly IMapper _mapper;

    public CareerController(IDataManager dataManager, IMapper mapper)
    {
        _dataManager = dataManager;
        _mapper = mapper;
    }

    [HttpPut]
    public async Task<IActionResult> CreateCareer(NewCareerDto career)
    {
        if (career is not null)
        {
            try
            {
                var model = _mapper.Map<CareerModel>(career);
                var result = await _dataManager.CreateCareerAsync(model);
                return result ? Ok(result) : Problem("Error while adding career to database.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        return BadRequest("You must provide a career.");
    }

    [HttpGet]
    public async Task<IActionResult> GetById(Guid id)
    {
        if (id != Guid.Empty)
        {
            try
            {
                var career = await _dataManager.GetCareerAsync(id);
                var dto = _mapper.Map<CareerDto>(career);
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
        return BadRequest("You must provide a career id.");
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetCareers(Guid facultyId)
    {
        if (facultyId != Guid.Empty)
        {
            try
            {
                var careers = await _dataManager.GetCareersAsync(facultyId);
                var dtos = careers.Select(c => _mapper.Map<CareerDto>(c)).ToList();
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
        return BadRequest("You must provide a faculty id.");
    }

    [HttpGet]
    [Route("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id)
    {
        try
        {
            var result = await _dataManager.ExistsCareerAsync(id);
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
            var careers = await _dataManager.GetCareersAsync(from, to);
            var dtos = careers.Select(c => _mapper.Map<CareerDto>(c)).ToList();
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
            var count = await _dataManager.GetCareersCountAsync();
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
        if (career is not null)
        {
            try
            {
                var model = _mapper.Map<CareerModel>(career);
                var result = await _dataManager.UpdateCareerAsync(model);
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
        return BadRequest("You must provide a faculty id.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCareer(Guid id)
    {
        if (id != Guid.Empty)
        {
            try
            {
                var result = await _dataManager.DeleteCareerAsync(id);
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
        return BadRequest("You must provide a career id.");
    }
}