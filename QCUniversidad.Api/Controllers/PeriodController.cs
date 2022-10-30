using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Curriculum;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class PeriodController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly IMapper _mapper;

    public PeriodController(IDataManager dataManager, IMapper mapper)
    {
        _dataManager = dataManager;
        _mapper = mapper;
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
            var result = await _dataManager.GetPeriodAsync(id);
            var dto = _mapper.Map<PeriodDto>(result);
            return Ok(dto);
        }
        catch (PeriodNotFoundException)
        {
            return NotFound($"The period with id {id} was not found.");
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        var periods = await _dataManager.GetPeriodsAsync(from, to);
        var dtos = periods.Select(d => _mapper.Map<PeriodDto>(d));
        return Ok(dtos);
    }

    [HttpGet("listbyschoolyear")]
    public async Task<IActionResult> GetListBySchoolYearAsync(Guid schoolYearId)
    {
        var periods = await _dataManager.GetPeriodsOfSchoolYearAsync(schoolYearId);
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
    public async Task<IActionResult> GetPeriodsCount(Guid schoolYearId)
    {
        try
        {
            var count = await _dataManager.GetSchoolYearPeriodsCountAsync(schoolYearId);
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
            try
            {
                var result = await _dataManager.UpdatePeriodAsync(_mapper.Map<PeriodModel>(period));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
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

    [HttpGet]
    [Route("planitem")]
    public async Task<IActionResult> GetPlanItemAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You must provide a plan item id.");
        }
        try
        {
            var result = await _dataManager.GetTeachingPlanItemAsync(id);
            var dto = _mapper.Map<TeachingPlanItemDto>(result);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("planitems")]
    public async Task<IActionResult> GetPlanItemsAsync(Guid periodId, int from = 0, int to = 0)
    {
        if (periodId == Guid.Empty)
        {
            return BadRequest("You must provide a period id.");
        }
        try
        {
            var result = await _dataManager.GetTeachingPlanItemsAsync(periodId, from, to);
            var dtos = result.Select(i => _mapper.Map<TeachingPlanItemSimpleDto>(i, opts => opts.AfterMap(async (o, planItem) =>
            {
                planItem.TotalLoadCovered = await _dataManager.GetPlanItemTotalCoveredAsync(planItem.Id);
                planItem.AllowLoad = planItem.TotalHoursPlanned > planItem.TotalLoadCovered;
            })));
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    [Route("addplanitem")]
    public async Task<IActionResult> AddPlanItemAsync(NewTeachingPlanItemDto teachingPlan)
    {
        if (teachingPlan is null)
        {
            return BadRequest("You must provide a teaching plan.");
        }
        if (teachingPlan.PeriodId == Guid.Empty)
        {
            return Problem("The teaching plan must have a period id.");
        }
        if (!await _dataManager.ExistsPeriodAsync(teachingPlan.PeriodId))
        {
            return Problem("The period of the teaching plan do not exist.");
        }
        try
        {
            var model = _mapper.Map<TeachingPlanItemModel>(teachingPlan);
            var result = await _dataManager.CreateTeachingPlanItemAsync(model);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("existsplanitem")]
    public async Task<IActionResult> ExistsPlanItemAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You must provide a valid id.");
        }
        try
        {
            var result = await _dataManager.ExistsTeachingPlanItemAsync(id);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("planitemscount")]
    public async Task<IActionResult> GetPlanItemsCount(Guid periodId)
    {
        if (periodId == Guid.Empty)
        {
            return BadRequest("You must provide a valid id.");
        }
        try
        {
            var result = await _dataManager.GetTeachingPlanItemsCountAsync(periodId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost]
    [Route("updateplanitem")]
    public async Task<IActionResult> UpdatePlanItem(EditTeachingPlanItemDto dto)
    {
        if (dto is null)
        {
            return BadRequest("You must provide a plan item model.");
        }
        if (!await _dataManager.ExistsTeachingPlanItemAsync(dto.Id))
        {
            return BadRequest($"The plan item with id {dto.PeriodId} do not exists.");
        }
        if (!await _dataManager.ExistsPeriodAsync(dto.PeriodId))
        {
            return BadRequest($"The period with id {dto.PeriodId} do not exists.");
        }
        try
        {
            var model = _mapper.Map<TeachingPlanItemModel>(dto);
            var result = await _dataManager.UpdateTeachingPlanItemAsync(model);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete]
    [Route("deletePlanItem")]
    public async Task<IActionResult> RemovePlanItem(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You must provide a valid id.");
        }
        try
        {
            if (!await _dataManager.ExistsTeachingPlanItemAsync(id))
            {
                return BadRequest("The teaching item do not exist.");
            }
            var result = await _dataManager.DeleteTeachingPlanItemAsync(id);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}