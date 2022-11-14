using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CourseController : ControllerBase
{
    private readonly IDataManager _dataManager;
    private readonly IMapper _mapper;

    public CourseController(IDataManager dataManager, IMapper mapper)
    {
        _dataManager = dataManager;
        _mapper = mapper;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
    {
        var courses = await _dataManager.GetCoursesAsync(from, to);
        var dtos = courses.Select(d => _mapper.Map<CourseDto>(d));
        return Ok(dtos);
    }

    [HttpGet]
    [Route("listbyschoolyear")]
    public async Task<IActionResult> GetListBySchoolYearAsync(Guid schoolYearId)
    {
        try
        {
            if (!await _dataManager.ExistSchoolYearAsync(schoolYearId))
            {
                return NotFound();
            }
            var result = await _dataManager.GetCoursesAsync(schoolYearId);
            var dtos = result.Select(c => _mapper.Map<CourseDto>(c)).OrderBy(dto => dto.CareerId).ThenBy(dto => dto.CareerYear);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("listbyschoolyearandfaculty")]
    public async Task<IActionResult> GetListBySchoolYearAndFacultyAsync(Guid schoolYearId, Guid facultyId)
    {
        try
        {
            if (!await _dataManager.ExistSchoolYearAsync(schoolYearId))
            {
                return NotFound();
            }
            if (!await _dataManager.ExistFacultyAsync(facultyId))
            {
                return NotFound();
            }
            var result = await _dataManager.GetCoursesAsync(schoolYearId, facultyId);
            var dtos = result.Select(c => _mapper.Map<CourseDto>(c)).OrderBy(dto => dto.CareerId).ThenBy(dto => dto.CareerYear);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("listbycareerschoolyearandfaculty")]
    public async Task<IActionResult> GetListByCareerSchoolYearAndFacultyAsync(Guid careerId, Guid schoolYearId, Guid facultyId)
    {
        try
        {
            if (!await _dataManager.ExistsCareerAsync(careerId))
            {
                return NotFound();
            }
            if (!await _dataManager.ExistSchoolYearAsync(schoolYearId))
            {
                return NotFound();
            }
            if (!await _dataManager.ExistFacultyAsync(facultyId))
            {
                return NotFound();
            }
            var result = await _dataManager.GetCoursesAsync(careerId, schoolYearId, facultyId);
            var dtos = result.Select(c => _mapper.Map<CourseDto>(c)).OrderBy(dto => dto.CareerId).ThenBy(dto => dto.CareerYear);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCount()
    {
        try
        {
            var count = await _dataManager.GetCoursesCountAsync();
            return Ok(count);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("periodscount")]
    public async Task<IActionResult> GetPeriodsCount(Guid schoolYearId)
    {
        try
        {
            var count = await _dataManager.GetSchoolYearPeriodsCountAsync(schoolYearId);
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
            var result = await _dataManager.ExistsCourseAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("existsbycareeryearandmodality")]
    public async Task<IActionResult> ExistsByCareerYearAndModality(Guid careerId, int careerYear, int modality)
    {
        if (careerId != Guid.Empty || careerYear >= 0 || modality >= 0)
        {
            var result = await _dataManager.CheckCourseExistenceByCareerYearAndModality(careerId, careerYear, (TeachingModality)modality);
            return Ok(result);
        }
        return BadRequest("The parameters should not be null.");
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(NewCourseDto course)
    {
        if (course is not null)
        {
            try
            {
                var model = _mapper.Map<CourseModel>(course);
                var result = await _dataManager.CreateCourseAsync(model);
                return result ? Ok(model.Id) : Problem("An error has occured creating the discipline.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        return BadRequest("The discipline cannot be null.");
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
            var result = await _dataManager.GetCourseAsync(id);
            var dto = _mapper.Map<CourseDto>(result);
            return Ok(dto);
        }
        catch (CourseNotFoundException)
        {
            return NotFound($"The school year with id {id} was not found.");
        }
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(EditCourseDto course)
    {
        if (course is not null)
        {
            var result = await _dataManager.UpdateCourseAsync(_mapper.Map<CourseModel>(course));
            return Ok(result);
        }
        return BadRequest("The school year cannot be null.");
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
            var result = await _dataManager.DeleteCourseAsync(id);
            return Ok(result);
        }
        catch (CourseNotFoundException)
        {
            return NotFound($"The school year with id '{id}' was not found.");
        }
    }

    [HttpGet]
    [Route("listfordepartment")]
    public async Task<IActionResult> GetCoursesForDepartment(Guid departmentId, Guid? schoolYearId = null)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest("You must provide an id.");
        }
        try
        {
            var result = await _dataManager.GetCoursesForDepartmentAsync(departmentId, schoolYearId);
            var dtos = result.Select(i => _mapper.Map<CourseDto>(i));
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}