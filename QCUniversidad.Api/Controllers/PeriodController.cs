using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.Api.Shared.Dtos.SchoolYear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PeriodController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly IMapper _mapper;

    public PeriodController(IDataManager dataManager, IMapper mapper)
    {
        _dataManager = dataManager;
        _mapper = mapper;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        var periods = await _dataManager.GetPeriodsAsync(from, to);
        var dtos = periods.Select(d => _mapper.Map<PeriodDto>(d));
        return Ok(dtos);
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCount()
    {
        try
        {
            var count = await _dataManager.GetPeriodsCountAsync();
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
            var result = await _dataManager.ExistsPeriodAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("existswithorder")]
    public async Task<IActionResult> ExistsAsync(Guid schoolYearId, int orderNumber)
    {
        try
        {
            var result = await _dataManager.ExistPeriodWithOrder(schoolYearId, orderNumber);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(NewPeriodDto period)
    {
        if (period is not null)
        {
            var model = _mapper.Map<PeriodModel>(period);
            var result = await _dataManager.CreatePeriodAsync(model);
            return result ? Ok(model.Id) : Problem("An error has occured creating the period.");
        }
        return BadRequest("The period cannot be null.");
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(EditPeriodDto period)
    {
        if (period is not null)
        {
            var result = await _dataManager.UpdatePeriodAsync(_mapper.Map<PeriodModel>(period));
            return Ok(result);
        }
        return BadRequest("The period cannot be null.");
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
            var result = await _dataManager.DeletePeriodAsync(id);
            return Ok(result);
        }
        catch (PeriodNotFoundException)
        {
            return NotFound($"The period with id '{id}' was not found.");
        }
    }
}
