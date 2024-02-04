using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Contracts;
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
public class TeacherController(ITeachersManager teachersManager,
                               IPeriodsManager periodsManager,
                               ITeachersLoadManager teachersLoadManager,
                               IMapper mapper) : ControllerBase
{
    private readonly ITeachersManager _teachersManager = teachersManager;
    private readonly IPeriodsManager _periodsManager = periodsManager;
    private readonly ITeachersLoadManager _teachersLoadManager = teachersLoadManager;
    private readonly IMapper _mapper = mapper;

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        IList<TeacherModel> teachers = await _teachersManager.GetTeachersAsync(from, to);
        IEnumerable<TeacherDto> dtos = teachers.Select(_mapper.Map<TeacherDto>);
        foreach (TeacherDto? dto in dtos)
        {
            TeacherModel teacher = teachers.First(t => t.Id == dto.Id);
            if ((teacher.TeacherDisciplines?.Any()) is false)
            {
                continue;
            }

            dto.Disciplines ??= new List<PopulatedDisciplineDto>();
            foreach (TeacherDiscipline td in teacher.TeacherDisciplines)
            {
                dto.Disciplines.Add(_mapper.Map<PopulatedDisciplineDto>(td.Discipline));
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
            int count = await _teachersManager.GetTeachersCountAsync();
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
            bool result = await _teachersManager.ExistsTeacherAsync(id);
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
            bool result = await _teachersManager.ExistsTeacherAsync(personalId);
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
        if (teacherDto is null)
        {
            return BadRequest("The teacher cannot be null.");
        }

        bool result = await _teachersManager.CreateTeacherAsync(_mapper.Map<TeacherModel>(teacherDto, opts => opts.AfterMap((o, t) =>
        {
            if (teacherDto.SelectedDisciplines?.Length is 0)
            {
                return;
            }

            t.TeacherDisciplines ??= new List<TeacherDiscipline>();
            foreach (Guid d in teacherDto.SelectedDisciplines)
            {
                t.TeacherDisciplines.Add(new TeacherDiscipline { DisciplineId = d });
            }
        })));

        return result ? Ok() : Problem("An error has occured creating the teacher.");
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
            TeacherModel result = await _teachersManager.GetTeacherAsync(id);
            TeacherDto dto = _mapper.Map<TeacherDto>(result);
            dto.Disciplines ??= new List<PopulatedDisciplineDto>();
            dto.Disciplines = result.TeacherDisciplines?
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
            TeacherModel result = await _teachersManager.GetTeacherAsync(personalId);
            TeacherDto dto = _mapper.Map<TeacherDto>(result);
            dto.Disciplines ??= new List<PopulatedDisciplineDto>();
            dto.Disciplines = result.TeacherDisciplines?
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
        if (teacher is null)
        {
            return BadRequest("The teacher cannot be null.");
        }

        TeacherModel model = _mapper.Map<TeacherModel>(teacher, opts => opts.AfterMap((o, t) =>
        {
            if ((teacher.SelectedDisciplines?.Any()) != true)
            {
                return;
            }

            t.TeacherDisciplines ??= new List<TeacherDiscipline>();
            foreach (Guid d in teacher.SelectedDisciplines)
            {
                t.TeacherDisciplines.Add(new TeacherDiscipline { DisciplineId = d, TeacherId = teacher.Id });
            }
        }));
        bool result = await _teachersManager.UpdateTeacherAsync(model);
        return Ok(result);
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
            bool result = await _teachersManager.DeleteTeacherAsync(id);
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
            IList<TeacherModel> result = await _teachersManager.GetTeachersOfDepartmentAsync(departmentId);
            IEnumerable<TeacherDto> dtos = result.Select(_mapper.Map<TeacherDto>);
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
            IList<TeacherModel> result = await _teachersManager.GetTeachersOfDepartmentAsync(departmentId);
            List<TeacherDto> dtos = [];
            foreach (TeacherModel teacher in result)
            {
                TeacherDto dto = _mapper.Map<TeacherDto>(teacher);
                double teacherTimeFund = await _teachersLoadManager.GetTeacherTimeFund(dto.Id, periodId);
                double load = await _teachersLoadManager.GetTeacherLoadInPeriodAsync(teacher.Id, periodId);
                double loadPercent = Math.Round(load / teacherTimeFund * 100, 2);
                TeacherLoadDto loadDto = new()
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
                    foreach (TeacherDiscipline td in teacher.TeacherDisciplines)
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
            IList<TeacherModel> result = await _teachersManager.GetTeachersOfDepartmentAsync(departmentId);
            List<TeacherDto> dtos = [];
            foreach (TeacherModel teacher in result)
            {
                TeacherDto dto = _mapper.Map<TeacherDto>(teacher);
                double teacherTimeFund = await _teachersLoadManager.GetTeacherTimeFund(dto.Id, periodId);
                double load = await _teachersLoadManager.GetTeacherLoadInPeriodAsync(teacher.Id, periodId);
                double loadPercent = Math.Round(load / teacherTimeFund * 100, 2);
                TeacherLoadDto loadDto = new()
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
                    foreach (TeacherDiscipline td in teacher.TeacherDisciplines)
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
            TeacherModel teacher = await _teachersManager.GetTeacherAsync(id);
            TeacherDto dto = _mapper.Map<TeacherDto>(teacher);
            double teacherTimeFund = await _teachersLoadManager.GetTeacherTimeFund(dto.Id, periodId);
            double load = await _teachersLoadManager.GetTeacherLoadInPeriodAsync(teacher.Id, periodId);
            double loadPercent = Math.Round(load / teacherTimeFund * 100, 2);
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
                foreach (TeacherDiscipline td in teacher.TeacherDisciplines)
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
            IList<TeacherModel> result = await _teachersLoadManager.GetTeachersOfDepartmentNotAssignedToPlanItemAsync(departmentId, planItemId, disciplineId);
            IEnumerable<TeacherDto> dtos = result.Select(_mapper.Map<TeacherDto>);
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
            IList<TeacherModel> result = await _teachersManager.GetSupportTeachersAsync(departmentId, periodId);
            IEnumerable<TeacherDto> dtos = result.Select(_mapper.Map<TeacherDto>);
            foreach (TeacherDto? dto in dtos)
            {
                TeacherModel t = result.First(teacher => teacher.Id == dto.Id);
                double teacherTimeFund = await _teachersLoadManager.GetTeacherTimeFund(t.Id, periodId);
                double load = await _teachersLoadManager.GetTeacherLoadInPeriodAsync(dto.Id, periodId);
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
                    foreach (TeacherDiscipline td in t.TeacherDisciplines)
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
            bool result = await _teachersLoadManager.SetLoadToTeacher(newLoadItem.TeacherId, newLoadItem.PlanningItemId, newLoadItem.HoursCovered);
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
            bool result = await _teachersLoadManager.DeleteLoadFromTeacherAsync(loadItemId);
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
            List<LoadViewItemDto> items = await GetTeacherLoadItemsAsync(id, periodId);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    private async Task<List<LoadViewItemDto>> GetTeacherLoadItemsAsync(Guid id, Guid periodId)
    {
        IList<LoadItemModel> teachingLoadItems = await _teachersLoadManager.GetTeacherLoadItemsInPeriodAsync(id, periodId);
        IEnumerable<LoadViewItemDto> teachingLoadViewItems = teachingLoadItems.Select(item => new LoadViewItemDto
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
        IList<NonTeachingLoadModel> nonTeachingLoadItems = await _teachersLoadManager.GetTeacherNonTeachingLoadItemsInPeriodAsync(id, periodId);
        bool recalculationAllowed = await _periodsManager.IsPeriodInCurrentYear(periodId);
        IEnumerable<LoadViewItemDto> nonTeachingLoadViewItems = nonTeachingLoadItems.Select(item => new LoadViewItemDto
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
        IEnumerable<NonTeachingLoadType> missingTypes = Enum.GetValues<NonTeachingLoadType>().Where(t => !nonTeachingLoadItems.Any(l => l.Type == t));
        IEnumerable<LoadViewItemDto> missingNonTeachingLoadItems = missingTypes.Select(type => new LoadViewItemDto
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
        List<LoadViewItemDto> items = [.. teachingLoadViewItems, .. nonTeachingLoadViewItems, .. missingNonTeachingLoadItems];
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

        if (!Enum.TryParse(typeof(NonTeachingLoadType), model.Type, out object? parsedType) || parsedType is null)
        {
            return BadRequest("The provided type is invalid.");
        }

        NonTeachingLoadType type = (NonTeachingLoadType)parsedType;
        try
        {
            bool result = await _teachersLoadManager.SetNonTeachingLoadAsync(type, model.BaseValue, model.TeacherId, model.PeriodId);
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

    [HttpPost]
    [Route("recalculateall")]
    public async Task<IActionResult> RecalculateAllTeachersInPeriod(Guid periodId)
    {
        try
        {
            await _teachersLoadManager.RecalculateAllTeachersLoadInPeriodAsync(periodId);
            return Ok();
        }
        catch
        {
            return Problem();
        }
    }
}