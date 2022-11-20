using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.Api.Shared.Dtos.LoadItem;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.Api.Shared.Extensions;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TeacherController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly IMapper _mapper;

    public TeacherController(IDataManager dataManager, IMapper mapper)
    {
        _dataManager = dataManager;
        _mapper = mapper;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        var teachers = await _dataManager.GetTeachersAsync(from, to);
        var dtos = teachers.Select(_mapper.Map<TeacherDto>);
        foreach (var dto in dtos)
        {
            var teacher = teachers.First(t => t.Id == dto.Id);
            if (teacher.TeacherDisciplines?.Any() == true)
            {
                dto.Disciplines ??= new List<PopulatedDisciplineDto>();
                foreach (var td in teacher.TeacherDisciplines)
                {
                    dto.Disciplines.Add(_mapper.Map<PopulatedDisciplineDto>(td.Discipline));
                }
            }
        }
        return Ok(dtos);
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCount()
    {
        try
        {
            var count = await _dataManager.GetTeachersCountAsync();
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
            var result = await _dataManager.ExistsTeacherAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("existsbypersonalid")]
    public async Task<IActionResult> ExistsAsync(string personalId)
    {
        try
        {
            var result = await _dataManager.ExistsTeacherAsync(personalId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(NewTeacherDto teacherDto)
    {
        if (teacherDto is not null)
        {
            var result = await _dataManager.CreateTeacherAsync(_mapper.Map<TeacherModel>(teacherDto, opts => opts.AfterMap((o, t) =>
            {
                if (teacherDto.SelectedDisciplines?.Any() == true)
                {
                    t.TeacherDisciplines ??= new List<TeacherDiscipline>();
                    foreach (var d in teacherDto.SelectedDisciplines)
                    {
                        t.TeacherDisciplines.Add(new TeacherDiscipline { DisciplineId = d });
                    }
                }
            })));
            return result ? Ok() : Problem("An error has occured creating the teacher.");
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
            var result = await _dataManager.GetTeacherAsync(id);
            var dto = _mapper.Map<TeacherDto>(result);
            dto.Disciplines ??= new List<PopulatedDisciplineDto>();
            dto.Disciplines = result.TeacherDisciplines
                                           .Select(td => _mapper.Map<PopulatedDisciplineDto>(td.Discipline))
                                           .ToList();
            return Ok(dto);
        }
        catch (TeacherNotFoundException)
        {
            return NotFound($"The teacher with id {id} was not found.");
        }
    }

    [HttpGet]
    [Route("bypersonalid")]
    public async Task<IActionResult> GetByPersonalIdAsync(string personalId)
    {
        if (!string.IsNullOrEmpty(personalId))
        {
            return BadRequest("You must provide an id.");
        }
        try
        {
            var result = await _dataManager.GetTeacherAsync(personalId);
            var dto = _mapper.Map<TeacherDto>(result);
            dto.Disciplines ??= new List<PopulatedDisciplineDto>();
            dto.Disciplines = result.TeacherDisciplines
                                           .Select(td => _mapper.Map<PopulatedDisciplineDto>(td.Discipline))
                                           .ToList();
            return Ok(dto);
        }
        catch (TeacherNotFoundException)
        {
            return NotFound($"The teacher with id {personalId} was not found.");
        }
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(EditTeacherDto teacher)
    {
        if (teacher is not null)
        {
            var model = _mapper.Map<TeacherModel>(teacher, opts => opts.AfterMap((o, t) =>
            {
                if (teacher.SelectedDisciplines?.Any() == true)
                {
                    t.TeacherDisciplines ??= new List<TeacherDiscipline>();
                    foreach (var d in teacher.SelectedDisciplines)
                    {
                        t.TeacherDisciplines.Add(new TeacherDiscipline { DisciplineId = d, TeacherId = teacher.Id });
                    }
                }
            }));
            var result = await _dataManager.UpdateTeacherAsync(model);
            return Ok(result);
        }
        return BadRequest("The teacher cannot be null.");
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
            var result = await _dataManager.DeleteTeacherAsync(id);
            return Ok(result);
        }
        catch (TeacherNotFoundException)
        {
            return NotFound($"The teacher with id '{id}' was not found.");
        }
    }

    [HttpGet]
    [Route("listofdepartment")]
    public async Task<IActionResult> GetTeachersOfDepartment(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest("You must provide a department id.");
        }
        try
        {
            var result = await _dataManager.GetTeachersOfDepartmentAsync(departmentId);
            var dtos = result.Select(_mapper.Map<TeacherDto>);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("listofdepartmentforperiod")]
    public async Task<IActionResult> GetTeachersOfDepartmentForPeriod(Guid departmentId, Guid periodId)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest("You must provide a department id.");
        }
        try
        {
            var result = await _dataManager.GetTeachersOfDepartmentAsync(departmentId);
            var dtos = new List<TeacherDto>();
            foreach (var teacher in result)
            {
                var dto = _mapper.Map<TeacherDto>(teacher);
                var teacherTimeFund = await _dataManager.GetTeacherTimeFund(dto.Id, periodId);
                var load = await _dataManager.GetTeacherLoadInPeriodAsync(teacher.Id, periodId);
                var loadPercent = Math.Round(load / teacherTimeFund * 100, 2);
                var loadDto = new TeacherLoadDto
                {
                    TeacherId = teacher.Id,
                    TimeFund = teacherTimeFund,
                    Load = load,
                    LoadPercent = Math.Round(load / teacherTimeFund * 100, 2),
                    Status = loadPercent switch
                    {
                        double p when p < 80 => TeacherLoadStatus.Underutilized,
                        double p when p is < 100 and >= 80 => TeacherLoadStatus.Acceptable,
                        double p when p == 100 => TeacherLoadStatus.Balanced,
                        _ => TeacherLoadStatus.Overloaded
                    },
                    PeriodId = periodId
                };
                dto.Load = loadDto;
                if (teacher.TeacherDisciplines?.Any() == true)
                {
                    dto.Disciplines ??= new List<PopulatedDisciplineDto>();
                    foreach (var td in teacher.TeacherDisciplines)
                    {
                        dto.Disciplines.Add(_mapper.Map<PopulatedDisciplineDto>(td.Discipline));
                    }
                }
                dtos.Add(dto);
            }
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("listofdepartmentforperiodwithloaditems")]
    public async Task<IActionResult> GetTeachersOfDepartmentForPeriodWithLoadItems(Guid departmentId, Guid periodId)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest("You must provide a department id.");
        }
        try
        {
            var result = await _dataManager.GetTeachersOfDepartmentAsync(departmentId);
            var dtos = new List<TeacherDto>();
            foreach (var teacher in result)
            {
                var dto = _mapper.Map<TeacherDto>(teacher);
                var teacherTimeFund = await _dataManager.GetTeacherTimeFund(dto.Id, periodId);
                var load = await _dataManager.GetTeacherLoadInPeriodAsync(teacher.Id, periodId);
                var loadPercent = Math.Round(load / teacherTimeFund * 100, 2);
                var loadDto = new TeacherLoadDto
                {
                    TeacherId = teacher.Id,
                    TimeFund = teacherTimeFund,
                    Load = load,
                    LoadPercent = Math.Round(load / teacherTimeFund * 100, 2),
                    Status = loadPercent switch
                    {
                        double p when p < 80 => TeacherLoadStatus.Underutilized,
                        double p when p is < 100 and >= 80 => TeacherLoadStatus.Acceptable,
                        double p when p == 100 => TeacherLoadStatus.Balanced,
                        _ => TeacherLoadStatus.Overloaded
                    },
                    PeriodId = periodId
                };
                dto.Load = loadDto;
                if (teacher.TeacherDisciplines?.Any() == true)
                {
                    dto.Disciplines ??= new List<PopulatedDisciplineDto>();
                    foreach (var td in teacher.TeacherDisciplines)
                    {
                        dto.Disciplines.Add(_mapper.Map<PopulatedDisciplineDto>(td.Discipline));
                    }
                }
                dto.LoadViewItems = await GetTeacherLoadItemsAsync(teacher.Id, periodId);
                dtos.Add(dto);
            }
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("withload")]
    public async Task<IActionResult> GetTeacherWithLoadAsync(Guid id, Guid periodId)
    {
        try
        {
            var teacher = await _dataManager.GetTeacherAsync(id);
            var dto = _mapper.Map<TeacherDto>(teacher);
            var teacherTimeFund = await _dataManager.GetTeacherTimeFund(dto.Id, periodId);
            var load = await _dataManager.GetTeacherLoadInPeriodAsync(teacher.Id, periodId);
            var loadPercent = Math.Round(load / teacherTimeFund * 100, 2);
            dto.Load = new TeacherLoadDto
            {
                TeacherId = teacher.Id,
                TimeFund = teacherTimeFund,
                Load = load,
                LoadPercent = Math.Round(load / teacherTimeFund * 100, 2),
                Status = loadPercent switch
                {
                    double p when p < 80 => TeacherLoadStatus.Underutilized,
                    double p when p is < 100 and >= 80 => TeacherLoadStatus.Acceptable,
                    double p when p == 100 => TeacherLoadStatus.Balanced,
                    _ => TeacherLoadStatus.Overloaded
                },
                PeriodId = periodId
            };
            if (teacher.TeacherDisciplines?.Any() == true)
            {
                dto.Disciplines ??= new List<PopulatedDisciplineDto>();
                foreach (var td in teacher.TeacherDisciplines)
                {
                    dto.Disciplines.Add(_mapper.Map<PopulatedDisciplineDto>(td.Discipline));
                }
            }
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("listofdepartmentnotinplanitem")]
    public async Task<IActionResult> GetTeachersOfDepartmentNotInLoadItem(Guid departmentId, Guid planItemId, Guid? disciplineId = null)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest("You must provide a department id.");
        }
        try
        {
            var result = await _dataManager.GetTeachersOfDepartmentNotAssignedToPlanItemAsync(departmentId, planItemId, disciplineId);
            var dtos = result.Select(_mapper.Map<TeacherDto>);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("listsupport")]
    public async Task<IActionResult> GetListSupport(Guid departmentId, Guid periodId)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest("You must provide a department id.");
        }
        if (periodId == Guid.Empty)
        {
            return BadRequest("You must provide a period id.");
        }
        try
        {
            var result = await _dataManager.GetSupportTeachersAsync(departmentId, periodId);
            var dtos = result.Select(_mapper.Map<TeacherDto>);
            foreach (var dto in dtos)
            {
                var t = result.First(teacher => teacher.Id == dto.Id);
                var teacherTimeFund = await _dataManager.GetTeacherTimeFund(t.Id, periodId);
                var load = await _dataManager.GetTeacherLoadInPeriodAsync(dto.Id, periodId);
                dto.Load = new TeacherLoadDto
                {
                    TeacherId = dto.Id,
                    TimeFund = teacherTimeFund,
                    Load = load,
                    LoadPercent = Math.Round(load / teacherTimeFund * 100, 2),
                    PeriodId = periodId
                };
                if (t.TeacherDisciplines?.Any() == true)
                {
                    dto.Disciplines ??= new List<PopulatedDisciplineDto>();
                    foreach (var td in t.TeacherDisciplines)
                    {
                        dto.Disciplines.Add(_mapper.Map<PopulatedDisciplineDto>(td.Discipline));
                    }
                }
            }
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    [Route("setload")]
    public async Task<IActionResult> SetLoadItem(NewLoadItemDto newLoadItem)
    {
        try
        {
            var result = await _dataManager.SetLoadToTeacher(newLoadItem.TeacherId, newLoadItem.PlanningItemId, newLoadItem.HoursCovered);
            return result ? Ok(result) : Problem();
        }
        catch (ArgumentNullException ex)
        {
            return Problem(ex.Message);
        }
        catch (TeacherNotFoundException)
        {
            return NotFound("Teacher not found.");
        }
        catch (TeachingPlanItemNotFoundException)
        {
            return NotFound("Plan item not found.");
        }
        catch (PlanItemFullyCoveredException)
        {
            return BadRequest("The plan item is already covered.");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("The hours should be greater than zero.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete]
    [Route("deleteload")]
    public async Task<IActionResult> DeleteLoadItem(Guid loadItemId)
    {
        try
        {
            var result = await _dataManager.DeleteLoadFromTeacherAsync(loadItemId);
            return result ? Ok(result) : Problem();
        }
        catch (LoadItemNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("loaditems")]
    public async Task<IActionResult> GetLoadItemsAsync(Guid id, Guid periodId)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Should provide a teacher id.");
        }
        try
        {
            var items = await GetTeacherLoadItemsAsync(id, periodId);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    private async Task<List<LoadViewItemDto>> GetTeacherLoadItemsAsync(Guid id, Guid periodId)
    {
        var teachingLoadItems = await _dataManager.GetTeacherLoadItemsInPeriodAsync(id, periodId);
        var teachingLoadViewItems = teachingLoadItems.Select(item => new LoadViewItemDto
        {
            LoadId = item.Id,
            Type = LoadViewItemType.Teaching,
            Status = LoadViewItemStatus.Setted,
            TeacherId = id,
            Teacher = item.Teacher is not null ? _mapper.Map<SimpleTeacherDto>(item.Teacher) : null,
            Name = item.PlanningItem.Subject?.Name,
            Description = $"{item.PlanningItem.Course?.Denomination} - {item.PlanningItem.Type.GetPlanItemTypeDisplayValue()} {item.PlanningItem.HoursPlanned}h x {item.PlanningItem.GroupsAmount} grupos",
            Value = item.HoursCovered,
            PeriodId = periodId
        });
        var nonTeachingLoadItems = await _dataManager.GetTeacherNonTeachingLoadItemsInPeriodAsync(id, periodId);
        var recalculationAllowed = await _dataManager.IsPeriodInCurrentYear(periodId);
        var nonTeachingLoadViewItems = nonTeachingLoadItems.Select(item => new LoadViewItemDto
        {
            LoadId = item.Id,
            Type = LoadViewItemType.NonTeaching,
            Autogenerated = item.Type.GetEnumDisplayAutogenerateValue(),
            Status = LoadViewItemStatus.Setted,
            Name = item.Type.GetEnumDisplayNameValue(),
            AllowRecalculation = recalculationAllowed,
            NonTeachingLoadType = item.Type,
            Description = item.Description,
            TeacherId = id,
            Value = item.Load,
            PeriodId = periodId
        });
        var missingTypes = Enum.GetValues<NonTeachingLoadType>().Where(t => !nonTeachingLoadItems.Any(l => l.Type == t));
        var missingNonTeachingLoadItems = missingTypes.Select(type => new LoadViewItemDto
        {
            Type = LoadViewItemType.NonTeaching,
            Autogenerated = type.GetEnumDisplayAutogenerateValue(),
            Status = LoadViewItemStatus.NotSetted,
            Name = type.GetEnumDisplayNameValue(),
            AllowRecalculation = recalculationAllowed,
            NonTeachingLoadType = type,
            Description = type.GetEnumDisplayDescriptionValue(),
            TeacherId = id,
            Value = 0,
            PeriodId = periodId
        });
        var items = new List<LoadViewItemDto>();
        items.AddRange(teachingLoadViewItems);
        items.AddRange(nonTeachingLoadViewItems);
        items.AddRange(missingNonTeachingLoadItems);
        return items;
    }

    [HttpPost]
    [Route("setnonteachingload")]
    public async Task<IActionResult> SetNonTeachingLoadAsync(SetNonTeachingLoadDto model)
    {
        if (model is null)
        {
            return BadRequest("No model provided.");
        }
        if (Enum.TryParse(typeof(NonTeachingLoadType), model.Type, out var parsedType))
        {
            if (parsedType is not null)
            {
                var type = (NonTeachingLoadType)parsedType;
                try
                {
                    var result = await _dataManager.SetNonTeachingLoadAsync(type, model.BaseValue, model.TeacherId, model.PeriodId);
                    return result ? Ok(result) : Problem();
                }
                catch (TeacherNotFoundException)
                {
                    return NotFound("The teacher was not found");
                }
                catch (PeriodNotFoundException)
                {
                    return NotFound("The period was not found");
                }
                catch (ArgumentNullException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (NonTeachingLoadUnsettableException)
                {
                    return BadRequest("The provided load type cannot be setted, it is autocalculated.");
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (ConfigurationException)
                {
                    return Problem("Error accessing the configuration values.");
                }
                catch (Exception ex)
                {
                    return Problem(ex.Message);
                }
            }
        }
        return BadRequest("The provided type is invalid.");
    }

    [HttpPost]
    [Route("recalculateall")]
    public async Task<IActionResult> RecalculateAllTeachersInPeriod(Guid periodId)
    {
        try
        {
            await _dataManager.RecalculateAllTeachersInPeriodAsync(periodId);
            return Ok();
        }
        catch
        {
            return Problem();
        }
    }
}