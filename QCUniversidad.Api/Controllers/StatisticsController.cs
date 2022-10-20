using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Statistics;
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

    public StatisticsController(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }

    [HttpGet]
    [Route("departmentstatistics")]
    public async Task<IActionResult> DepartmentStatisticsAsync(Guid departmentId, Guid periodId)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest();
        }
        if (!await _dataManager.ExistDepartmentAsync(departmentId))
        {
            return NotFound();
        }
        var items = new List<StatisticItemDto>();

        var teachersCount = new StatisticItemDto 
        {
            Name = "Cantidad de profesores",
            Mu = "",
            Value = await _dataManager.GetDeparmentTeachersCountAsync(departmentId),
            State = Shared.Enums.StatisticState.Ok,
            Description = "Cantidad de profesores del departamento"
        };
        items.Add(teachersCount);

        return Ok(items);
    }
}
