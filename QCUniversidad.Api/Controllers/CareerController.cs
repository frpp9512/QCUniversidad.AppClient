using MediatR;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CareerController(IMediator mediator) : ApiControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPut]
    public async Task<IActionResult> CreateCareer(NewCareerDto career, CancellationToken cancellationToken)
    {
        if (career is null)
        {
            return BadRequest("You must provide the career data.");
        }

        var request = new CreateCareerRequest { NewCareerDto = career };
        var response = await _mediator.Send(request, cancellationToken);
        return GetCreatedResponseResult(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetCareerById(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetCareerByIdRequest { CareerId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetCareers(Guid facultyId, CancellationToken cancellationToken)
    {
        var request = new GetCareersForFacultyRequest { FacultyId = facultyId };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("listfordepartment")]
    public async Task<IActionResult> GetCareersForDepartmentAsync(Guid departmentId, CancellationToken cancellationToken)
    {
        var request = new GetCareersForDepartmentRequest { DepartmentId = departmentId };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new ExistsCareerRequest { CareerId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("listall")]
    public async Task<IActionResult> GetCareers(int from = 0, int to = 0, CancellationToken cancellationToken = default)
    {
        var request = new GetCareersRangeRequest { From = from, To = to };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("countall")]
    public async Task<IActionResult> GetCount(CancellationToken cancellationToken)
    {
        var request = new GetCareersCountRequest();
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> UpdateCareer(EditCareerDto career, CancellationToken cancellationToken)
    {
        if (career is null)
        {
            return BadRequest("You must provide a career data.");
        }

        var request = new UpdateCareerRequest { Career = career };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCareer(Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteCareerRequest { CareerId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }
}