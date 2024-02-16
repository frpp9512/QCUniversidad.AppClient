using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PeriodController(IPeriodsManager periodsManager,
                              IPlanningManager planningManager,
                              IMapper mapper) : ControllerBase
{
    private readonly IPeriodsManager _periodsManager = periodsManager;
    private readonly IPlanningManager _planningManager = planningManager;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You must provide an id.");
        }

        try
        {
            PeriodModel result = await _periodsManager.GetPeriodAsync(id);
            PeriodDto dto = _mapper.Map<PeriodDto>(result);
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
        IList<PeriodModel> periods = await _periodsManager.GetPeriodsAsync(from, to);
        IEnumerable<PeriodDto> dtos = periods.Select(_mapper.Map<PeriodDto>);
        return Ok(dtos);
    }

    [HttpGet("listbyschoolyear")]
    public async Task<IActionResult> GetListBySchoolYearAsync(Guid schoolYearId)
    {
        IList<PeriodModel> periods = await _periodsManager.GetPeriodsOfSchoolYearAsync(schoolYearId);
        IEnumerable<PeriodDto> dtos = periods.Select(_mapper.Map<PeriodDto>);
        return Ok(dtos);
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCount()
    {
        try
        {
            int count = await _periodsManager.GetPeriodsCountAsync();
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
            int count = await _periodsManager.GetSchoolYearPeriodsCountAsync(schoolYearId);
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
            bool result = await _periodsManager.ExistsPeriodAsync(id);
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
            PeriodModel model = _mapper.Map<PeriodModel>(period);
            bool result = await _periodsManager.CreatePeriodAsync(model);
            return result ? Ok(model.Id) : Problem("An error has occured creating the period.");
        }

        return BadRequest("The period cannot be null.");
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(EditPeriodDto period)
    {
        if (period is null)
        {
            return BadRequest("The period cannot be null.");
        }

        try
        {
            bool result = await _periodsManager.UpdatePeriodAsync(_mapper.Map<PeriodModel>(period));
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
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
            bool result = await _periodsManager.DeletePeriodAsync(id);
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
            TeachingPlanItemModel result = await _planningManager.GetTeachingPlanItemAsync(id);
            TeachingPlanItemDto dto = _mapper.Map<TeachingPlanItemDto>(result);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("planitems")]
    public async Task<IActionResult> GetPlanItemsAsync(Guid periodId, Guid? courseId = null, int from = 0, int to = 0)
    {
        if (periodId == Guid.Empty)
        {
            return BadRequest("You must provide a period id.");
        }

        try
        {
            IList<TeachingPlanItemModel> result = await _planningManager.GetTeachingPlanItemsAsync(periodId, courseId, from, to);
            List<TeachingPlanItemSimpleDto> dtos = [];
            foreach (TeachingPlanItemModel item in result)
            {
                TeachingPlanItemSimpleDto dto = _mapper.Map<TeachingPlanItemSimpleDto>(item);
                dto.TotalLoadCovered = await _planningManager.GetPlanItemTotalCoveredAsync(item.Id);
                dto.AllowLoad = dto.TotalHoursPlanned > dto.TotalLoadCovered;
                dtos.Add(dto);
            }

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

        if (!await _periodsManager.ExistsPeriodAsync(teachingPlan.PeriodId))
        {
            return Problem("The period of the teaching plan do not exist.");
        }

        try
        {
            TeachingPlanItemModel model = _mapper.Map<TeachingPlanItemModel>(teachingPlan);
            bool result = await _planningManager.CreateTeachingPlanItemAsync(model);
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
            bool result = await _planningManager.ExistsTeachingPlanItemAsync(id);
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
            int result = await _planningManager.GetTeachingPlanItemsCountAsync(periodId);
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

        if (!await _planningManager.ExistsTeachingPlanItemAsync(dto.Id))
        {
            return BadRequest($"The plan item with id {dto.PeriodId} do not exists.");
        }

        if (!await _periodsManager.ExistsPeriodAsync(dto.PeriodId))
        {
            return BadRequest($"The period with id {dto.PeriodId} do not exists.");
        }

        try
        {
            TeachingPlanItemModel model = _mapper.Map<TeachingPlanItemModel>(dto);
            bool result = await _planningManager.UpdateTeachingPlanItemAsync(model);
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
            if (!await _planningManager.ExistsTeachingPlanItemAsync(id))
            {
                return BadRequest("The teaching item do not exist.");
            }

            bool result = await _planningManager.DeleteTeachingPlanItemAsync(id);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}