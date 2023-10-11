using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.Api.Shared.Dtos.Statistics;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly IMapper _mapper;
    private readonly CalculationOptions _calculationOptions;

    public DepartmentController(IDataManager dataManager, IMapper mapper, IOptions<CalculationOptions> options)
    {
        _dataManager = dataManager;
        _mapper = mapper;
        _calculationOptions = options.Value;
    }

    [HttpGet]
    [Route("listall")]
    public async Task<IActionResult> GetList(int from = 0, int to = 0)
    {
        var deparments = await _dataManager.GetDepartmentsAsync(from, to);
        var dtos = deparments.Select(_mapper.Map<DepartmentDto>).ToList();
        foreach (var dto in dtos)
        {
            dto.DisciplinesCount = await _dataManager.GetDepartmentDisciplinesCount(dto.Id);
        }

        return Ok(dtos);
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetList(Guid facultyId)
    {
        if (facultyId == Guid.Empty)
        {
            return BadRequest("You should provide a faculty id to load the departments from.");
        }

        var deparments = await _dataManager.GetDepartmentsAsync(facultyId);
        var dtos = deparments.Select(_mapper.Map<DepartmentDto>).ToList();
        foreach (var dto in dtos)
        {
            dto.DisciplinesCount = await _dataManager.GetDepartmentDisciplinesCount(dto.Id);
        }

        return Ok(dtos);
    }

    [HttpGet]
    [Route("listallwithload")]
    public async Task<IActionResult> GetListWithLoad(Guid periodId)
    {
        try
        {
            var deparments = await _dataManager.GetDepartmentsAsync();
            var dtos = deparments.Select(_mapper.Map<DepartmentDto>).ToList();
            foreach (var dto in dtos)
            {
                dto.DisciplinesCount = await _dataManager.GetDepartmentDisciplinesCount(dto.Id);
                var load = await _dataManager.GetDepartmentTotalLoadInPeriodAsync(periodId, dto.Id);
                dto.Load = load;
                var loadCovered = await _dataManager.GetDepartmentTotalLoadCoveredInPeriodAsync(periodId, dto.Id);
                dto.LoadCovered = loadCovered;
                dto.LoadCoveredPercent = load == 0 ? 0 : Math.Round(loadCovered / load * 100, 1);
                var totalFund = await _dataManager.GetDepartmentTotalTimeFund(dto.Id, periodId);
                dto.TotalTimeFund = totalFund;
                dto.LoadPercent = totalFund == 0 ? 0 : Math.Round(load / totalFund * 100, 1);
            }

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("countall")]
    public async Task<IActionResult> GetCount()
    {
        try
        {
            var count = await _dataManager.GetDepartmentsCountAsync();
            return Ok(count);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("countdisciplines")]
    public async Task<IActionResult> GetDisciplinesCount(Guid departmentId)
    {
        try
        {
            var count = await _dataManager.GetDepartmentsCountAsync(departmentId);
            return Ok(count);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCount(Guid facultyId)
    {
        try
        {
            var count = await _dataManager.GetDepartmentsCountAsync(facultyId);
            return Ok(count);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id)
    {
        try
        {
            var result = await _dataManager.ExistDepartmentAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You should provide a department id.");
        }

        try
        {
            var department = await _dataManager.GetDepartmentAsync(id);
            var dto = _mapper.Map<DepartmentDto>(department);
            return Ok(dto);
        }
        catch (FacultyNotFoundException)
        {
            return NotFound($"The department with id '{id}' was not found.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> CreateDeparment(NewDepartmentDto department)
    {
        if (department is null)
        {
            return BadRequest("You should provide a department.");
        }

        try
        {
            var model = _mapper.Map<DepartmentModel>(department);
            var result = await _dataManager.CreateDepartmentAsync(model);
            return result ? Ok(result) : (IActionResult)Problem("Error while adding department to database.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> UpdateDepartment(EditDepartmentDto department)
    {
        if (department is null)
        {
            return BadRequest("You should provide a department.");
        }

        var model = _mapper.Map<DepartmentModel>(department);
        var result = await _dataManager.UpdateDeparmentAsync(model);
        return result ? Ok(result) : (IActionResult)Problem("Error while updating department in database.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDepartment(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You should provide a department id.");
        }

        var result = await _dataManager.DeleteDeparmentAsync(id);
        return result ? Ok(result) : (IActionResult)Problem("Error while deleting department from database.");
    }

    [HttpGet]
    [Route("planningitems")]
    public async Task<IActionResult> GetPlanningItems(Guid id, Guid periodId, bool onlyLoadItems = false, Guid? courseId = null)
    {
        try
        {
            var result = await _dataManager.GetTeachingPlanItemsOfDepartmentOnPeriod(id, periodId, courseId, onlyLoadItems);
            var periodTimeFund = await _dataManager.GetPeriodTimeFund(periodId);
            var dtos = result.Select(_mapper.Map<TeachingPlanItemDto>).ToList();
            foreach (var dto in dtos)
            {
                if (dto.LoadItems is null)
                {
                    continue;
                }

                foreach (var loadItem in dto.LoadItems)
                {
                    if (loadItem.Teacher is null)
                    {
                        continue;
                    }

                    var load = await _dataManager.GetTeacherLoadInPeriodAsync(loadItem.Teacher.Id, periodId);
                    loadItem.Teacher.Load = new TeacherLoadDto
                    {
                        TeacherId = loadItem.Teacher.Id,
                        TimeFund = periodTimeFund,
                        Load = load,
                        LoadPercent = Math.Round(load / periodTimeFund * 100, 2),
                        PeriodId = periodId
                    };
                }
            }

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("periodstats")]
    public async Task<IActionResult> GetPeriodStatistics(Guid departmentId, Guid periodId)
    {
        if (periodId == Guid.Empty)
        {
            return BadRequest(new { error = "You should provide a valid period id." });
        }

        if (departmentId == Guid.Empty)
        {
            return BadRequest(new { error = "You should provide a valid department id." });
        }

        try
        {
            List<StatisticItemDto> stats = new();
            var period = await _dataManager.GetPeriodAsync(periodId);
            var timeFund = period.TimeFund;

            stats.Add(new()
            {
                Name = "Fondo de tiempo del período",
                Mu = "h",
                Value = timeFund
            });

            var teachersCount = await _dataManager.GetTeachersCountAsync();
            stats.Add(new()
            {
                Name = "Cantidad de profesores",
                Mu = "U",
                Value = teachersCount
            });

            var salary = teachersCount * _calculationOptions.AverageMonthlySalary * period.MonthsCount;
            stats.Add(new()
            {
                Name = "Salario promedio",
                Mu = "CUP",
                Value = salary
            });

            var depCapacity = timeFund * teachersCount;
            stats.Add(new()
            {
                Name = "Capacidad del departamento",
                Mu = "h-profesor/período",
                Value = depCapacity
            });

            var depLoad = await _dataManager.GetDepartmentTotalLoadInPeriodAsync(periodId, departmentId);
            stats.Add(new()
            {
                Name = "Carga del departamento",
                Mu = "h-profesor/período",
                Value = depLoad
            });

            var depLoadPercent = Math.Round(depLoad / depCapacity * 100, 2);
            stats.Add(new()
            {
                Name = "Porciento de carga",
                Mu = "Porciento (%)",
                Value = depLoadPercent
            });

            var diff = depLoad - depCapacity;
            var personalRequiriement = Math.Floor(diff / (_calculationOptions.MonthTimeFund * period.MonthsCount));
            stats.Add(new()
            {
                Name = "Ajustes de personal",
                Mu = "U",
                Value = personalRequiriement
            });

            var salaryImpact = personalRequiriement * _calculationOptions.AverageMonthlySalary * period.MonthsCount;
            stats.Add(new()
            {
                Name = "Imacto económico luego de ajustes de personal",
                Mu = "CUP",
                Value = salaryImpact
            });

            var rap = await _dataManager.CalculateRAPAsync(departmentId);
            stats.Add(new()
            {
                Name = "Relación alumno-profesor",
                Mu = "",
                Value = rap
            });

            return Ok(stats);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message);
        }
    }
}