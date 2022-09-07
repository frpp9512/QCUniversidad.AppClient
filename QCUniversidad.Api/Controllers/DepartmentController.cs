using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos;
using System;
using System.Collections.Generic;
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
        [Route("list")]
        public async Task<IActionResult> GetList(Guid facultyId)
        {
            if (facultyId != Guid.Empty)
            {
                var deparments = await _dataManager.GetDepartmentsAsync(facultyId);
                var dtos = deparments.Select(d => _mapper.Map<DepartmentDto>(d));
                return Ok(dtos);
            }
            return BadRequest("You should provide a faculty id to load the departments from.");
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id != Guid.Empty)
            {
                try
                {
                    var department = await _dataManager.GetDeparmentAsync(id);
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
        public async Task<IActionResult> CreateDeparment(DepartmentDto department)
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
        public async Task<IActionResult> UpdateDepartment(DepartmentDto department)
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