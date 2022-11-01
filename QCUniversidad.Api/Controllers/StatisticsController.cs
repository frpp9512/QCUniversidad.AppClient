using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Statistics;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly CalculationOptions _calculationOptions;

    public StatisticsController(IDataManager dataManager, IOptions<CalculationOptions> options)
    {
        _dataManager = dataManager;
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
                    double i when i > 0.8 && i < 1 => StatisticState.Low,
                    double i when i == 1 => StatisticState.Ok,
                    double i when i > 1 && i < 1.5 => StatisticState.High,
                    double i when i > 1.5 => StatisticState.TooHigh,
                    _ => StatisticState.Ok,
                },
                Description = "Relación de profesores por estudiante"
            };
            items.Add(rap);

            var currentSchoolYear = await _dataManager.GetCurrentSchoolYear();
            var periods = currentSchoolYear.Periods;
            foreach (var period in periods.OrderBy(p => p.Starts))
            {
                var periodLoad = await _dataManager.GetDepartmentTotalLoadCoveredInPeriodAsync(period.Id, departmentId);
                var periodTimeFund = period.TimeFund;
                var percent = periodLoad / periodTimeFund;
                var state = percent switch
                {
                    double i when i < 0.6 => StatisticState.TooLow,
                    double i when i < 0.8 && i >= 0.6 => StatisticState.Low,
                    double i when i <= 1 && i >= 0.8 => StatisticState.Ok,
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
                    Name = $"Carga del período {period.ToString()}",
                    Value = periodLoad,
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
}
