using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Statistics;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly IMapper _mapper;
    private readonly CalculationOptions _calculationOptions;

    public StatisticsController(IDataManager dataManager, IOptions<CalculationOptions> options, IMapper mapper)
    {
        _dataManager = dataManager;
        _mapper = mapper;
        _calculationOptions = options.Value;
    }

    [HttpGet]
    [Route("globalstatistics")]
    public async Task<IActionResult> GlobalStatistics()
    {
        var items = new List<StatisticItemDto>();

        var faculties = new StatisticItemDto
        {
            Name = "Facultades",
            Description = "Cantidad de facultades en el sistema",
            Value = await _dataManager.GetFacultiesTotalAsync(),
        };
        items.Add(faculties);

        var departments = new StatisticItemDto
        {
            Name = "Departamentos",
            Description = "Cantidad de departamentos en el sistema",
            Value = await _dataManager.GetDepartmentsCountAsync(),
        };
        items.Add(departments);

        var teachers = new StatisticItemDto
        {
            Name = "Profesores",
            Description = "Cantidad de profesores en el sistema",
            Value = await _dataManager.GetTeachersCountAsync(),
        };
        items.Add(teachers);

        var disciplines = new StatisticItemDto
        {
            Name = "Disciplinas",
            Description = "Cantidad de facultades en el sistema",
            Value = await _dataManager.GetDisciplinesCountAsync(),
        };
        items.Add(disciplines);

        var subjects = new StatisticItemDto
        {
            Name = "Asignaturas",
            Description = "Cantidad de asignaturas en el sistema",
            Value = await _dataManager.GetSubjectsCountAsync(),
        };
        items.Add(subjects);

        var curriculums = new StatisticItemDto
        {
            Name = "Curriculums",
            Description = "Cantidad de curriculums en el sistema",
            Value = await _dataManager.GetFacultiesTotalAsync(),
        };
        items.Add(curriculums);

        var schoolYears = new StatisticItemDto
        {
            Name = "Años escolares",
            Description = "Cantidad de años escolares en el sistema",
            Value = await _dataManager.GetFacultiesTotalAsync(),
        };
        items.Add(schoolYears);

        var courses = new StatisticItemDto
        {
            Name = "Cursos",
            Description = "Cantidad de cursos en el sistema",
            Value = await _dataManager.GetFacultiesTotalAsync(),
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
        if (!await _dataManager.ExistDepartmentAsync(departmentId))
        {
            return NotFound();
        }
        try
        {
            var items = new List<StatisticItemDto>();

            var disciplines = new StatisticItemDto
            {
                Name = "Disciplinas",
                Description = "Cantidad de disciplinas que gestiona el departamento",
                Value = await _dataManager.GetDepartmentDisciplinesCount(departmentId),
            };
            items.Add(disciplines);

            var teachersCount = new StatisticItemDto
            {
                Name = "Cantidad de profesores",
                Value = await _dataManager.GetDeparmentTeachersCountAsync(departmentId),
                Description = "Cantidad de profesores del departamento"
            };
            items.Add(teachersCount);

            var rapValue = await _dataManager.CalculateRAPAsync(departmentId);
            var referencePercent = rapValue / _calculationOptions.RAPReference;
            var rap = new StatisticItemDto
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

            var currentSchoolYear = await _dataManager.GetCurrentSchoolYearAsync();
            var periods = currentSchoolYear.Periods;
            foreach (var period in periods.OrderBy(p => p.Starts))
            {
                var periodLoad = await _dataManager.GetDepartmentTotalLoadCoveredInPeriodAsync(period.Id, departmentId);
                var periodTotalTimeFund = teachersCount.Value * period.TimeFund;
                var totalPercent = periodLoad / periodTotalTimeFund;
                var totalState = totalPercent switch
                {
                    double i when i < 0.6 => StatisticState.TooLow,
                    double i when i is < 0.8 and >= 0.6 => StatisticState.Low,
                    double i when i is <= 1 and >= 0.8 => StatisticState.Ok,
                    _ => StatisticState.TooHigh
                };
                var totalDescription = totalState switch
                {
                    StatisticState.TooLow => "La fuerza de trabajo esta altamente desaprovechada",
                    StatisticState.Low => "La fuerza de trabajo esta desaprovechada",
                    StatisticState.Ok => "Existe un aprovechamiento adecuado de la fuerza de trabajo",
                    StatisticState.High => "Existe sobrecarga de la fuerza de trabajo",
                    StatisticState.TooHigh => "Existe sobrecarga de la fuerza de trabajo",
                    _ => "Carga del departamento referente al período."
                };
                var periodTotalLoadStat = new StatisticItemDto
                {
                    Name = $"Carga total del período {period}",
                    Value = periodLoad,
                    RefValue = periodTotalTimeFund,
                    State = totalState,
                    Description = totalDescription
                };
                items.Add(periodTotalLoadStat);

                var workForceUtilization = new StatisticItemDto
                {
                    Name = $"Aprovechamiento de la FT {period}",
                    Value = Math.Round(totalPercent * 100, 2),
                    State = totalState,
                    Description = totalDescription,
                    Mu = "%"
                };
                items.Add(workForceUtilization);

                var averagePeriodLoad = await _dataManager.GetDepartmentAverageTotalLoadCoveredInPeriodAsync(period.Id, departmentId);
                var periodTimeFund = period.TimeFund;
                var percent = averagePeriodLoad / periodTimeFund;
                var state = percent switch
                {
                    double i when i < 0.6 => StatisticState.TooLow,
                    double i when i is < 0.8 and >= 0.6 => StatisticState.Low,
                    double i when i is <= 1 and >= 0.8 => StatisticState.Ok,
                    _ => StatisticState.TooHigh
                };
                var description = state switch
                {
                    StatisticState.TooLow => "La fuerza de trabajo esta altamente desaprovechada",
                    StatisticState.Low => "La fuerza de trabajo esta desaprovechada",
                    StatisticState.Ok => "Existe un aprovechamiento adecuado de la fuerza de trabajo",
                    StatisticState.High => "Existe sobrecarga de la fuerza de trabajo",
                    StatisticState.TooHigh => "Existe sobrecarga de la fuerza de trabajo",
                    _ => "Carga del departamento referente al período."
                };
                var periodLoadStat = new StatisticItemDto
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
        var today = DateTime.Now;
        var firstDayOfWeek = today.AddDays(((int)today.DayOfWeek - 1) * -1);
        var lastDayOfWeek = today.AddDays(7 - (int)today.DayOfWeek);
        var departmentTeachers = await _dataManager.GetTeachersOfDepartmentAsync(departmentId);
        var birthdays = departmentTeachers.Where(teacher => (teacher.Birthday?.Month >= firstDayOfWeek.Month && teacher.Birthday?.Month <= lastDayOfWeek.Month) && (teacher.Birthday?.Day >= firstDayOfWeek.Day && teacher.Birthday?.Day <= lastDayOfWeek.Day));
        var dtos = birthdays.Select(bt => _mapper.Map<BirthdayTeacherDto>(bt));
        return Ok(dtos);
    }

    [HttpGet]
    [Route("monthbirthdaysfordepartment")]
    public async Task<IActionResult> GetBirthdayTeachersForCurrentMonthAsync(Guid departmentId)
    {
        var today = DateTime.Now;
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
        var lastDayOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        var departmentTeachers = await _dataManager.GetTeachersOfDepartmentAsync(departmentId);
        var birthdays = departmentTeachers.Where(teacher => (teacher.Birthday?.Month >= firstDayOfMonth.Month && teacher.Birthday?.Month <= lastDayOfMonth.Month) && (teacher.Birthday?.Day >= firstDayOfMonth.Day && teacher.Birthday?.Day <= lastDayOfMonth.Day));
        var dtos = birthdays.Select(bt => _mapper.Map<BirthdayTeacherDto>(bt)).OrderBy(dtos => dtos.Birthday.Month).ThenBy(dto => dto.Birthday.Day);
        return Ok(dtos);
    }
}