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

            return Ok(items);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
