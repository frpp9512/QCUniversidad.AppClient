using MediatR;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Requests.Curriculums.Models;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CurriculumController(IMediator mediator) : ApiControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0, CancellationToken cancellationToken = default)
    {
        var request = new GetCurriculumsRangeRequest { From = from, To = to };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet("listforcareer")]
    public async Task<IActionResult> GetListForCareerAsync(Guid careerId, CancellationToken cancellationToken)
    {
        var request = new GetCurriculumsForCareerRequest { CareerId = careerId };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCount(CancellationToken cancellationToken)
    {
        var request = new GetCurriculumsCountRequest();
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new ExistCurriculumRequest { CurriculumId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(NewCurriculumDto curriculumDto, CancellationToken cancellationToken)
    {
        var request = new CreateCurriculumRequest { NewCurriculum = curriculumDto };
        var response = await _mediator.Send(request, cancellationToken);
        return GetCreatedResponseResult(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetCurriculumByIdRequest { CurriculumId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(EditCurriculumDto curriculum, CancellationToken cancellationToken)
    {
        var request = new UpdateCurriculumRequest { CurriculumToUpdate = curriculum };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteCurriculumRequest { CurriculumId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }
}
