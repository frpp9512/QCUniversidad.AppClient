using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Services;
using QCUniversidad.Api.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DisciplineController : ControllerBase
    {
        private readonly IDataManager _dataManager;
        private readonly IMapper _mapper;

        public DisciplineController(IDataManager dataManager, IMapper mapper)
        {
            _dataManager = dataManager;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetListAsync(int from = 0, int to = 0)
        {
            var disciplines = await _dataManager.GetDisciplinesAsync(from, to);
            var dtos = disciplines.Select(d => _mapper.Map<DisciplineDto>(d));
            foreach (var dto in dtos)
            {
                dto.TeachersCount = await _dataManager.GetDisciplineTeachersCountAsync(dto.Id);
                dto.SubjectsCount = await _dataManager.GetDisciplineSubjectsCountAsync(dto.Id);
            }
            return Ok(dtos);
        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetCount()
        {
            try
            {
                var count = await _dataManager.GetDisciplinesCountAsync();
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
                var result = await _dataManager.ExistsDisciplineAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> CreateAsync(NewDisciplineDto disciplineDto)
        {
            if (disciplineDto is not null)
            {
                var result = await _dataManager.CreateDisciplineAsync(_mapper.Map<DisciplineModel>(disciplineDto));
                return result ? Ok() : Problem("An error has occured creating the discipline.");
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
                var result = await _dataManager.GetDisciplineAsync(id);
                var dto = _mapper.Map<DisciplineDto>(result);
                dto.SubjectsCount = await _dataManager.GetDisciplineSubjectsCountAsync(dto.Id);
                dto.TeachersCount = await _dataManager.GetDisciplineTeachersCountAsync(dto.Id);
                return Ok(dto);
            }
            catch (DisciplineNotFoundException)
            {
                return NotFound($"The discipline with id {id} was not found.");
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(EditDisciplineDto discipline)
        {
            if (discipline is not null)
            {
                var result = await _dataManager.UpdateDisciplineAsync(_mapper.Map<DisciplineModel>(discipline));
                return Ok(result);
            }
            return BadRequest("The discipline cannot be null.");
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
                var result = await _dataManager.DeleteDisciplineAsync(id);
                return Ok(result);
            }
            catch (DisciplineNotFoundException)
            {
                return NotFound($"The discipline with id '{id}' was not found.");
            }
        }
    }
}
