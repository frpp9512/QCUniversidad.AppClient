using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.Api.Shared.Dtos.Statistics;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController(IDepartmentsManager dataManager,
                                  IPeriodsManager periodsManager,
                                  IPlanningManager planningManager,
                                  ITeachersManager teachersManager,
                                  ITeachersLoadManager teachersLoadManager,
                                  IMapper mapper,
                                  IOptions<CalculationOptions> options) : ControllerBase
{
    private readonly IDepartmentsManager _departmentsManager = dataManager;
    private readonly IPeriodsManager _periodsManager = periodsManager;
    private readonly IPlanningManager _planningManager = planningManager;
    private readonly ITeachersManager _teachersManager = teachersManager;
    private readonly ITeachersLoadManager _teachersLoadManager = teachersLoadManager;
    private readonly IMapper _mapper = mapper;
    private readonly CalculationOptions _calculationOptions = options.Value;

    [HttpGet]
    [Route("listall")]
    public async Task<IActionResult> GetList(int from = 0, int to = 0)
    {
        IList<DepartmentModel> deparments = await _departmentsManager.GetDepartmentsAsync(from, to);
        List<DepartmentDto> dtos = deparments.Select(_mapper.Map<DepartmentDto>).ToList();
        foreach (DepartmentDto? dto in dtos)
        {
            dto.DisciplinesCount = await _departmentsManager.GetDepartmentDisciplinesCount(dto.Id);
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

        IList<DepartmentModel> deparments = await _departmentsManager.GetDepartmentsAsync(facultyId);
        List<DepartmentDto> dtos = deparments.Select(_mapper.Map<DepartmentDto>).ToList();
        foreach (DepartmentDto? dto in dtos)
        {
            dto.DisciplinesCount = await _departmentsManager.GetDepartmentDisciplinesCount(dto.Id);
        }

        return Ok(dtos);
    }

    [HttpGet]
    [Route("listallwithload")]
    public async Task<IActionResult> GetListWithLoad(Guid periodId)
    {
        try
        {
            IList<DepartmentModel> deparments = await _departmentsManager.GetDepartmentsAsync();
            List<DepartmentDto> dtos = deparments.Select(_mapper.Map<DepartmentDto>).ToList();
            foreach (DepartmentDto? dto in dtos)
            {
                dto.DisciplinesCount = await _departmentsManager.GetDepartmentDisciplinesCount(dto.Id);
                double load = await _departmentsManager.GetDepartmentTotalLoadInPeriodAsync(periodId, dto.Id);
                dto.Load = load;
                double loadCovered = await _departmentsManager.GetDepartmentTotalLoadCoveredInPeriodAsync(periodId, dto.Id);
                dto.LoadCovered = loadCovered;
                dto.LoadCoveredPercent = load == 0 ? 0 : Math.Round(loadCovered / load * 100, 1);
                double totalFund = await _departmentsManager.GetDepartmentTotalTimeFund(dto.Id, periodId);
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
            int count = await _departmentsManager.GetDepartmentsCountAsync();
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
            int count = await _departmentsManager.GetDepartmentsCountAsync(departmentId);
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
            int count = await _departmentsManager.GetDepartmentsCountAsync(facultyId);
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
            bool result = await _departmentsManager.ExistDepartmentAsync(id);
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
            DepartmentModel department = await _departmentsManager.GetDepartmentAsync(id);
            DepartmentDto dto = _mapper.Map<DepartmentDto>(department);
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
            DepartmentModel model = _mapper.Map<DepartmentModel>(department);
            bool result = await _departmentsManager.CreateDepartmentAsync(model);
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

        DepartmentModel model = _mapper.Map<DepartmentModel>(department);
        bool result = await _departmentsManager.UpdateDeparmentAsync(model);
        return result ? Ok(result) : (IActionResult)Problem("Error while updating department in database.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDepartment(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("You should provide a department id.");
        }

        bool result = await _departmentsManager.DeleteDepartmentAsync(id);
        return result ? Ok(result) : (IActionResult)Problem("Error while deleting department from database.");
    }

    [HttpGet]
    [Route("planningitems")]
    public async Task<IActionResult> GetPlanningItems(Guid id, Guid periodId, bool onlyLoadItems = false, Guid? courseId = null)
    {
        try
        {
            IList<TeachingPlanItemModel> result = await _planningManager.GetTeachingPlanItemsOfDepartmentOnPeriod(id, periodId, courseId, onlyLoadItems);
            double periodTimeFund = await _periodsManager.GetPeriodTimeFund(periodId);
            List<TeachingPlanItemDto> dtos = result.Select(_mapper.Map<TeachingPlanItemDto>).ToList();
            foreach (TeachingPlanItemDto? dto in dtos)
            {
                if (dto.LoadItems is null)
                {
                    continue;
                }

                foreach (Shared.Dtos.LoadItem.SimpleLoadItemDto loadItem in dto.LoadItems)
                {
                    if (loadItem.Teacher is null)
                    {
                        continue;
                    }

                    double load = await _teachersLoadManager.GetTeacherLoadInPeriodAsync(loadItem.Teacher.Id, periodId);
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
            List<StatisticItemDto> stats = [];
            PeriodModel period = await _periodsManager.GetPeriodAsync(periodId);
            double timeFund = period.TimeFund;

            stats.Add(new()
            {
                Name = "Fondo de tiempo del período",
                Mu = "h",
                Value = timeFund
            });

            int teachersCount = await _teachersManager.GetTeachersCountAsync();
            stats.Add(new()
            {
                Name = "Cantidad de profesores",
                Mu = "U",
                Value = teachersCount
            });

            double salary = teachersCount * _calculationOptions.AverageMonthlySalary * period.MonthsCount;
            stats.Add(new()
            {
                Name = "Salario promedio",
                Mu = "CUP",
                Value = salary
            });

            double depCapacity = timeFund * teachersCount;
            stats.Add(new()
            {
                Name = "Capacidad del departamento",
                Mu = "h-profesor/período",
                Value = depCapacity
            });

            double depLoad = await _departmentsManager.GetDepartmentTotalLoadInPeriodAsync(periodId, departmentId);
            stats.Add(new()
            {
                Name = "Carga del departamento",
                Mu = "h-profesor/período",
                Value = depLoad
            });

            double depLoadPercent = Math.Round(depLoad / depCapacity * 100, 2);
            stats.Add(new()
            {
                Name = "Porciento de carga",
                Mu = "Porciento (%)",
                Value = depLoadPercent
            });

            double diff = depLoad - depCapacity;
            double personalRequiriement = Math.Floor(diff / (_calculationOptions.MonthTimeFund * period.MonthsCount));
            stats.Add(new()
            {
                Name = "Ajustes de personal",
                Mu = "U",
                Value = personalRequiriement
            });

            double salaryImpact = personalRequiriement * _calculationOptions.AverageMonthlySalary * period.MonthsCount;
            stats.Add(new()
            {
                Name = "Imacto económico luego de ajustes de personal",
                Mu = "CUP",
                Value = salaryImpact
            });

            double rap = await _departmentsManager.CalculateRAPAsync(departmentId);
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