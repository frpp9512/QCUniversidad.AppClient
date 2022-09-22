using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QCUniversidad.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly IDataManager _dataManager;
        private readonly IMapper _mapper;

        public TeacherController(IDataManager dataManager, IMapper mapper)
        {
            _dataManager = dataManager;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
        {
            var teachers = await _dataManager.GetTeachersAsync(from, to);
            var dtos = teachers.Select(d => _mapper.Map<TeacherDto>(d, opt => opt.AfterMap((o, t) => 
            {
                if (d.TeacherDisciplines?.Any() == true)
                {
                    t.Disciplines ??= new List<PopulatedDisciplineDto>();
                    foreach (var td in d.TeacherDisciplines)
                    {
                        t.Disciplines.Add(_mapper.Map<PopulatedDisciplineDto>(td.Discipline));
                    } 
                }
            })));
            return Ok(dtos);
        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetCount()
        {
            try
            {
                var count = await _dataManager.GetTeachersCountAsync();
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
                var result = await _dataManager.ExistsTeacherAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> CreateAsync(NewTeacherDto teacherDto)
        {
            if (teacherDto is not null)
            {
                var result = await _dataManager.CreateTeacherAsync(_mapper.Map<TeacherModel>(teacherDto, opts => opts.AfterMap((o, t) => 
                {
                    if (teacherDto.SelectedDisciplines?.Any() == true)
                    {
                        t.TeacherDisciplines ??= new List<TeacherDiscipline>();
                        foreach (var d in teacherDto.SelectedDisciplines)
                        {
                            t.TeacherDisciplines.Add(new TeacherDiscipline { DisciplineId = d });
                        }
                    }
                })));
                return result ? Ok() : Problem("An error has occured creating the teacher.");
            }
            return BadRequest("The teacher cannot be null.");
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
                var result = await _dataManager.GetTeacherAsync(id);
                var dto = _mapper.Map<TeacherDto>(result);
                dto.Disciplines ??= new List<PopulatedDisciplineDto>();
                dto.Disciplines = result.TeacherDisciplines
                                               .Select(td => _mapper.Map<PopulatedDisciplineDto>(td.Discipline))
                                               .ToList();
                return Ok(dto);
            }
            catch (TeacherNotFoundException)
            {
                return NotFound($"The teacher with id {id} was not found.");
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(EditTeacherDto teacher)
        {
            if (teacher is not null)
            {
                var model = _mapper.Map<TeacherModel>(teacher, opts => opts.AfterMap((o, t) =>
                {
                    if (teacher.SelectedDisciplines?.Any() == true)
                    {
                        t.TeacherDisciplines ??= new List<TeacherDiscipline>();
                        foreach (var d in teacher.SelectedDisciplines)
                        {
                            t.TeacherDisciplines.Add(new TeacherDiscipline { DisciplineId = d, TeacherId = teacher.Id });
                        }
                    }
                }));
                var result = await _dataManager.UpdateTeacherAsync(model);
                return Ok(result);
            }
            return BadRequest("The teacher cannot be null.");
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
                var result = await _dataManager.DeleteTeacherAsync(id);
                return Ok(result);
            }
            catch (TeacherNotFoundException)
            {
                return NotFound($"The teacher with id '{id}' was not found.");
            }
        }
    }
}
