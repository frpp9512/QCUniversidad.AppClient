﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Controllers
{
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
            var dtos = deparments.Select(d => _mapper.Map<DepartmentDto>(d, opts => opts.AfterMap(async (o, d) =>
            {
                d.DisciplinesCount = await _dataManager.GetDepartmentDisciplinesCount(d.Id);
            })));
            return Ok(dtos);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetList(Guid facultyId)
        {
            if (facultyId != Guid.Empty)
            {
                var deparments = await _dataManager.GetDepartmentsAsync(facultyId);
                var dtos = deparments.Select(d => _mapper.Map<DepartmentDto>(d, opts => opts.AfterMap(async (o, d) => 
                {
                    d.DisciplinesCount = await _dataManager.GetDepartmentDisciplinesCount(d.Id);
                })));
                return Ok(dtos);
            }
            return BadRequest("You should provide a faculty id to load the departments from.");
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
    }
}