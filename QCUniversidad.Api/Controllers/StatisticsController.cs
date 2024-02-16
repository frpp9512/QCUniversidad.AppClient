using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Statistics;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StatisticsController(ISchoolYearsManager dataManager,
                                  IFacultiesManager facultiesManager,
                                  IDepartmentsManager departmentsManager,
                                  ITeachersManager teachersManager,
                                  IDisciplinesManager disciplinesManager,
                                  ISubjectsManager subjectsManager,
                                  ICurriculumsManager curriculumsManager,
                                  ICoursesManager coursesManager,
                                  IOptions<CalculationOptions> options,
                                  IMapper mapper) : ControllerBase
{
    private readonly ISchoolYearsManager _schoolsYearManager = dataManager;
    private readonly IFacultiesManager _facultiesManager = facultiesManager;
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;
    private readonly ITeachersManager _teachersManager = teachersManager;
    private readonly IDisciplinesManager _disciplinesManager = disciplinesManager;
    private readonly ISubjectsManager _subjectsManager = subjectsManager;
    private readonly ICurriculumsManager _curriculumsManager = curriculumsManager;
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly IMapper _mapper = mapper;
    private readonly CalculationOptions _calculationOptions = options.Value;

    [HttpGet]
    [Route("globalstatistics")]
    public async Task<IActionResult> GlobalStatistics()
    {
        List<StatisticItemDto> items = [];

        StatisticItemDto faculties = new()
        {
            Name = "Facultades",
            Description = "Cantidad de facultades en el sistema",
            Value = await _facultiesManager.GetFacultiesTotalAsync(),
        };
        items.Add(faculties);

        StatisticItemDto departments = new()
        {
            Name = "Departamentos",
            Description = "Cantidad de departamentos en el sistema",
            Value = await _departmentsManager.GetDepartmentsCountAsync(),
        };
        items.Add(departments);

        StatisticItemDto teachers = new()
        {
            Name = "Profesores",
            Description = "Cantidad de profesores en el sistema",
            Value = await _teachersManager.GetTeachersCountAsync(),
        };
        items.Add(teachers);

        StatisticItemDto disciplines = new()
        {
            Name = "Disciplinas",
            Description = "Cantidad de disiplinas en el sistema",
            Value = await _disciplinesManager.GetDisciplinesCountAsync(),
        };
        items.Add(disciplines);

        StatisticItemDto subjects = new()
        {
            Name = "Asignaturas",
            Description = "Cantidad de asignaturas en el sistema",
            Value = await _subjectsManager.GetSubjectsCountAsync(),
        };
        items.Add(subjects);

        StatisticItemDto curriculums = new()
        {
            Name = "Curriculums",
            Description = "Cantidad de curriculums en el sistema",
            Value = await _curriculumsManager.GetCurriculumsCountAsync(),
        };
        items.Add(curriculums);

        StatisticItemDto schoolYears = new()
        {
            Name = "Años escolares",
            Description = "Cantidad de años escolares en el sistema",
            Value = await _schoolsYearManager.GetSchoolYearTotalAsync(),
        };
        items.Add(schoolYears);

        StatisticItemDto courses = new()
        {
            Name = "Cursos",
            Description = "Cantidad de cursos en el sistema",
            Value = await _coursesManager.GetCoursesCountAsync(),
        };
        items.Add(courses);

        return Ok(items);
    }

    [HttpGet]
    [Route("departmentstatistics")]
    public async Task<IActionResult> DepartmentStatisticsAsync(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest();
        }

        if (!await _departmentsManager.ExistDepartmentAsync(departmentId))
        {
            return NotFound();
        }

        try
        {
            List<StatisticItemDto> items = [];

            StatisticItemDto disciplines = new()
            {
                Name = "Disciplinas",
                Description = "Cantidad de disciplinas que gestiona el departamento",
                Value = await _departmentsManager.GetDepartmentDisciplinesCount(departmentId),
            };
            items.Add(disciplines);

            StatisticItemDto teachersCount = new()
            {
                Name = "Cantidad de profesores",
                Value = await _departmentsManager.GetDepartmentTeachersCountAsync(departmentId),
                Description = "Cantidad de profesores del departamento"
            };
            items.Add(teachersCount);

            double rapValue = await _departmentsManager.CalculateRAPAsync(departmentId);
            double referencePercent = rapValue / _calculationOptions.RAPReference;
            StatisticItemDto rap = new()
            {
                Name = "Relación alumno-profesor",
                Value = rapValue,
                RefValue = _calculationOptions.RAPReference,
                State = referencePercent switch
                {
                    double i when i < 0.8 => StatisticState.TooLow,
                    double i when i is > 0.8 and < 1 => StatisticState.Low,
                    double i when i == 1 => StatisticState.Ok,
                    double i when i is > 1 and < 1.5 => StatisticState.High,
                    double i when i > 1.5 => StatisticState.TooHigh,
                    _ => StatisticState.Ok,
                },
                Description = "Relación de profesores por estudiante"
            };
            items.Add(rap);

            SchoolYearModel currentSchoolYear = await _schoolsYearManager.GetCurrentSchoolYearAsync();
            IList<PeriodModel> periods = currentSchoolYear.Periods;
            foreach (PeriodModel? period in periods.OrderBy(p => p.Starts))
            {
                double periodLoad = await _departmentsManager.GetDepartmentTotalLoadCoveredInPeriodAsync(period.Id, departmentId);
                double periodTotalTimeFund = teachersCount.Value * period.TimeFund;
                double totalPercent = periodLoad / periodTotalTimeFund;
                StatisticState totalState = totalPercent switch
                {
                    double i when i < 0.6 => StatisticState.TooLow,
                    double i when i is < 0.8 and >= 0.6 => StatisticState.Low,
                    double i when i is <= 1 and >= 0.8 => StatisticState.Ok,
                    _ => StatisticState.TooHigh
                };
                string totalDescription = totalState switch
                {
                    StatisticState.TooLow => "La fuerza de trabajo esta altamente desaprovechada",
                    StatisticState.Low => "La fuerza de trabajo esta desaprovechada",
                    StatisticState.Ok => "Existe un aprovechamiento adecuado de la fuerza de trabajo",
                    StatisticState.High => "Existe sobrecarga de la fuerza de trabajo",
                    StatisticState.TooHigh => "Existe sobrecarga de la fuerza de trabajo",
                    _ => "Carga del departamento referente al período."
                };
                StatisticItemDto periodTotalLoadStat = new()
                {
                    Name = $"Carga total del período {period}",
                    Value = periodLoad,
                    RefValue = periodTotalTimeFund,
                    State = totalState,
                    Description = totalDescription
                };
                items.Add(periodTotalLoadStat);

                StatisticItemDto workForceUtilization = new()
                {
                    Name = $"Aprovechamiento de la FT {period}",
                    Value = Math.Round(totalPercent * 100, 2),
                    State = totalState,
                    Description = totalDescription,
                    Mu = "%"
                };
                items.Add(workForceUtilization);

                double averagePeriodLoad = await _departmentsManager.GetDepartmentAverageTotalLoadCoveredInPeriodAsync(period.Id, departmentId);
                double periodTimeFund = period.TimeFund;
                double percent = averagePeriodLoad / periodTimeFund;
                StatisticState state = percent switch
                {
                    double i when i < 0.6 => StatisticState.TooLow,
                    double i when i is < 0.8 and >= 0.6 => StatisticState.Low,
                    double i when i is <= 1 and >= 0.8 => StatisticState.Ok,
                    _ => StatisticState.TooHigh
                };
                string description = state switch
                {
                    StatisticState.TooLow => "La fuerza de trabajo esta altamente desaprovechada",
                    StatisticState.Low => "La fuerza de trabajo esta desaprovechada",
                    StatisticState.Ok => "Existe un aprovechamiento adecuado de la fuerza de trabajo",
                    StatisticState.High => "Existe sobrecarga de la fuerza de trabajo",
                    StatisticState.TooHigh => "Existe sobrecarga de la fuerza de trabajo",
                    _ => "Carga del departamento referente al período."
                };
                StatisticItemDto periodLoadStat = new()
                {
                    Name = $"Carga promedio del período {period}",
                    Value = averagePeriodLoad,
                    RefValue = periodTimeFund,
                    State = state,
                    Description = description
                };
                items.Add(periodLoadStat);
            }

            return Ok(items);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("weekbirthdaysfordepartment")]
    public async Task<IActionResult> GetBirthdayTeachersForCurrentWeekAsync(Guid departmentId)
    {
        DateTime today = DateTime.Now;
        DateTime firstDayOfWeek = today.AddDays(((int)today.DayOfWeek - 1) * -1);
        DateTime lastDayOfWeek = today.AddDays(7 - (int)today.DayOfWeek);
        IList<TeacherModel> departmentTeachers = await _teachersManager.GetTeachersOfDepartmentAsync(departmentId);
        IEnumerable<TeacherModel> birthdays = departmentTeachers.Where(teacher => teacher.Birthday?.Month >= firstDayOfWeek.Month && teacher.Birthday?.Month <= lastDayOfWeek.Month && teacher.Birthday?.Day >= firstDayOfWeek.Day && teacher.Birthday?.Day <= lastDayOfWeek.Day);
        IEnumerable<BirthdayTeacherDto> dtos = birthdays.Select(_mapper.Map<BirthdayTeacherDto>);
        return Ok(dtos);
    }

    [HttpGet]
    [Route("monthbirthdaysfordepartment")]
    public async Task<IActionResult> GetBirthdayTeachersForCurrentMonthAsync(Guid departmentId)
    {
        DateTime today = DateTime.Now;
        DateTime firstDayOfMonth = new(today.Year, today.Month, 1);
        DateTime lastDayOfMonth = new(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        IList<TeacherModel> departmentTeachers = await _teachersManager.GetTeachersOfDepartmentAsync(departmentId);
        IEnumerable<TeacherModel> birthdays = departmentTeachers.Where(teacher => teacher.Birthday?.Month >= firstDayOfMonth.Month && teacher.Birthday?.Month <= lastDayOfMonth.Month && teacher.Birthday?.Day >= firstDayOfMonth.Day && teacher.Birthday?.Day <= lastDayOfMonth.Day);
        IOrderedEnumerable<BirthdayTeacherDto> dtos = birthdays.Select(_mapper.Map<BirthdayTeacherDto>).OrderBy(dtos => dtos.Birthday.Month).ThenBy(dto => dto.Birthday.Day);
        return Ok(dtos);
    }

    [HttpGet]
    [Route("weekbirthdaysforscope")]
    public async Task<IActionResult> GetWeekBirthdayTeachersForScopeAsync(string scope, Guid scopeId)
    {
        DateTime today = DateTime.Now;
        DateTime firstDayOfWeek = today.AddDays(((int)today.DayOfWeek - 1) * -1);
        DateTime lastDayOfWeek = today.AddDays(7 - (int)today.DayOfWeek);
        IList<TeacherModel> teachers;
        switch (scope)
        {
            case "department":
                teachers = await _teachersManager.GetTeachersOfDepartmentAsync(scopeId);
                break;
            case "faculty":
                teachers = await _teachersManager.GetTeachersOfFacultyAsync(scopeId);
                break;
            case "global":
                teachers = await _teachersManager.GetTeachersAsync();
                break;
            default:
                return BadRequest();
        }

        IEnumerable<TeacherModel> birthdays = teachers.Where(teacher => teacher.Birthday?.Month >= firstDayOfWeek.Month && teacher.Birthday?.Month <= lastDayOfWeek.Month && teacher.Birthday?.Day >= firstDayOfWeek.Day && teacher.Birthday?.Day <= lastDayOfWeek.Day);
        IEnumerable<BirthdayTeacherDto> dtos = birthdays.Select(_mapper.Map<BirthdayTeacherDto>);
        return Ok(dtos);
    }

    [HttpGet]
    [Route("monthbirthdaysforscope")]
    public async Task<IActionResult> GetMonthBirthdayTeachersForScopeAsync(string scope, Guid scopeId)
    {
        DateTime today = DateTime.Now;
        DateTime firstDayOfMonth = new(today.Year, today.Month, 1);
        DateTime lastDayOfMonth = new(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        IList<TeacherModel> teachers;
        switch (scope)
        {
            case "department":
                teachers = await _teachersManager.GetTeachersOfDepartmentAsync(scopeId);
                break;
            case "faculty":
                teachers = await _teachersManager.GetTeachersOfFacultyAsync(scopeId);
                break;
            case "global":
                teachers = await _teachersManager.GetTeachersAsync();
                break;
            default:
                return BadRequest();
        }

        IEnumerable<TeacherModel> birthdays = teachers.Where(teacher => teacher.Birthday?.Month >= firstDayOfMonth.Month && teacher.Birthday?.Month <= lastDayOfMonth.Month && teacher.Birthday?.Day >= firstDayOfMonth.Day && teacher.Birthday?.Day <= lastDayOfMonth.Day);
        IOrderedEnumerable<BirthdayTeacherDto> dtos = birthdays.Select(_mapper.Map<BirthdayTeacherDto>).OrderBy(dtos => dtos.Birthday.Month).ThenBy(dto => dto.Birthday.Day);
        return Ok(dtos);
    }
}