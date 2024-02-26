using MediatR;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Requests.Faculties.Models;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FacultyController(IMediator mediator) : ApiControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0, CancellationToken cancellationToken = default)
    {
        var request = new GetFacultiesRangeRequest { From = from, To = to };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet("count")]
    public async Task<IActionResult> CountAsync(CancellationToken cancellationToken)
    {
        var request = new GetFacultiesCountRequest();
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new ExistFacultyRequest { FacultyId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(FacultyDto facultyDto, CancellationToken cancellationToken)
    {
        var request = new CreateFacultyRequest { Faculty = facultyDto };
        var response = await _mediator.Send(request, cancellationToken);
        return GetCreatedResponseResult(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetFacultyByIdRequest { FacultyId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(FacultyDto faculty, CancellationToken cancellationToken)
    {
        var request = new UpdateFacultyRequest { Faculty = faculty };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFaculty(Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteFacultyRequest { FacultyId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }
}