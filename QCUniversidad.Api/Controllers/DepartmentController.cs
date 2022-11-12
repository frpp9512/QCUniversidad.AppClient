using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class DepartmentController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly IMapper _mapper;

    public DepartmentController(IDataManager dataManager, IMapper mapper)
    {
        _dataManager = dataManager;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("listall")]
    public async Task<IActionResult> GetList(int from = 0, int to = 0)
    {
        var deparments = await _dataManager.GetDepartmentsAsync(from, to);
        var dtos = deparments.Select(d => _mapper.Map<DepartmentDto>(d));
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
        if (facultyId != Guid.Empty)
        {
            var deparments = await _dataManager.GetDepartmentsAsync(facultyId);
            var dtos = deparments.Select(d => _mapper.Map<DepartmentDto>(d));
            foreach (var dto in dtos)
            {
                dto.DisciplinesCount = await _dataManager.GetDepartmentDisciplinesCount(dto.Id);
            }
            return Ok(dtos);
        }
        return BadRequest("You should provide a faculty id to load the departments from.");
    }

    [HttpGet]
    [Route("listallwithload")]
    public async Task<IActionResult> GetListWithLoad(Guid periodId)
    {
        try
        {
            var deparments = await _dataManager.GetDepartmentsAsync();
            var dtos = deparments.Select(d => _mapper.Map<DepartmentDto>(d));
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
        if (id != Guid.Empty)
        {
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
        return BadRequest("You should provide a department id.");
    }

    [HttpPut]
    public async Task<IActionResult> CreateDeparment(NewDepartmentDto department)
    {
        if (department is not null)
        {
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
        return BadRequest("You should provide a department.");
    }

    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> UpdateDepartment(EditDepartmentDto department)
    {
        if (department is not null)
        {
            var model = _mapper.Map<DepartmentModel>(department);
            var result = await _dataManager.UpdateDeparmentAsync(model);
            return result ? Ok(result) : (IActionResult)Problem("Error while updating department in database.");
        }
        return BadRequest("You should provide a department.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDepartment(Guid id)
    {
        if (id != Guid.Empty)
        {
            var result = await _dataManager.DeleteDeparmentAsync(id);
            return result ? Ok(result) : (IActionResult)Problem("Error while deleting department from database.");
        }
        return BadRequest("You should provide a department id.");
    }

    [HttpGet]
    [Route("planningitems")]
    public async Task<IActionResult> GetPlanningItems(Guid id, Guid periodId, Guid? courseId = null)
    {
        try
        {
            var result = await _dataManager.GetTeachingPlanItemsOfDepartmentOnPeriod(id, periodId, courseId);
            var periodTimeFund = await _dataManager.GetPeriodTimeFund(periodId);
            var dtos = result.Select(i => _mapper.Map<TeachingPlanItemDto>(i));
            foreach (var dto in dtos)
            {
                if (dto.LoadItems is not null)
                {
                    foreach (var loadItem in dto.LoadItems)
                    {
                        if (loadItem.Teacher is not null)
                        {
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
                }
            }
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}