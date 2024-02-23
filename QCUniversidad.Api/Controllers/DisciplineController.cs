using MediatR;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Requests.Disciplines.Models;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DisciplineController(IMediator mediator) : ApiControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0, CancellationToken cancellationToken = default)
    {
        var request = new GetDisciplinesRangeRequest { From = from, To = to };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("listofdepartment")]
    public async Task<IActionResult> GetListOfDepartmentAsync(Guid departmentId, CancellationToken cancellationToken)
    {
        var request = new GetDisciplinesOfDepartmentRequest { DepartmentId = departmentId };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCount(CancellationToken cancellationToken)
    {
        var request = new GetDisciplinesCountRequest();
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new ExistDisciplineRequest { DisciplineId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("existsbyname")]
    public async Task<IActionResult> ExistsAsync(string name, CancellationToken cancellationToken)
    {
        var request = new ExistDisciplineWithNameRequest { Name = name };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(NewDisciplineDto disciplineDto, CancellationToken cancellationToken)
    {
        var request = new CreateDisciplineRequest { NewDiscipline = disciplineDto };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetDisciplineByIdRequest { DisciplineId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("byname")]
    public async Task<IActionResult> GetByIdAsync(string name, CancellationToken cancellationToken)
    {
        var request = new GetDisciplineByNameRequest { DisciplineName = name };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(EditDisciplineDto discipline, CancellationToken cancellationToken)
    {
        var request = new UpdateDisciplineRequest { DisciplineToUpdate = discipline };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteDisciplineRequest { DisciplineId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }
}
