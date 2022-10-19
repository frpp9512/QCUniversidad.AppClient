using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Curriculum;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.Api.Shared.Dtos.Subject;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CurriculumController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly IMapper _mapper;

    public CurriculumController(IDataManager dataManager, IMapper mapper)
    {
        _dataManager = dataManager;
        _mapper = mapper;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        var curriculums = await _dataManager.GetCurriculumsAsync(from, to);
        var dtos = curriculums.Select(d => _mapper.Map<CurriculumDto>(d));
        return Ok(dtos);
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCount()
    {
        try
        {
            var count = await _dataManager.GetCurriculumsCountAsync();
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
            var result = await _dataManager.ExistsCurriculumAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(NewCurriculumDto curriculumDto)
    {
        if (curriculumDto is not null)
        {
            try
            {
                var model = _mapper.Map<CurriculumModel>(curriculumDto);
                var result = await _dataManager.CreateCurriculumAsync(_mapper.Map<CurriculumModel>(curriculumDto));
                return result ? Ok() : Problem("An error has occured creating the curriculum.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        return BadRequest("The teacher cannot be null.");
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
            var result = await _dataManager.GetCurriculumAsync(id);
            var dto = _mapper.Map<CurriculumDto>(result);
            dto.CurriculumDisciplines ??= new List<SimpleDisciplineDto>();
            dto.CurriculumDisciplines = result.CurriculumDisciplines
                                           .Select(cs => _mapper.Map<SimpleDisciplineDto>(cs.Discipline))
                                           .ToList();
            return Ok(dto);
        }
        catch (CurriculumNotFoundException)
        {
            return NotFound($"The curriculum with id {id} was not found.");
        }
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(EditCurriculumDto curriculum)
    {
        if (curriculum is not null)
        {
            var model = _mapper.Map<CurriculumModel>(curriculum);
            var result = await _dataManager.UpdateCurriculumAsync(model);
            return Ok(result);
        }
        return BadRequest("The curriculum cannot be null.");
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
            var result = await _dataManager.DeleteCurriculumAsync(id);
            return Ok(result);
        }
        catch (CurriculumNotFoundException)
        {
            return NotFound($"The curriculum with id '{id}' was not found.");
        }
    }
}
