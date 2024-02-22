using MediatR;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Planning.Models;
using QCUniversidad.Api.Requests.Statistics.Models;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController(IMediator mediator) : ApiControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [Route("listall")]
    public async Task<IActionResult> GetList(int from = 0, int to = 0, CancellationToken cancellationToken = default)
    {
        var request = new GetDepartmentsRangeRequest { From = from, To = to };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetList(Guid facultyId, CancellationToken cancellationToken)
    {
        var request = new GetDepartmentsOfFacultyRequest { FacultyId = facultyId };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("listallwithload")]
    public async Task<IActionResult> GetListWithLoad(Guid periodId, CancellationToken cancellationToken)
    {
        var request = new GetDepartmentsWithLoadInfoInPeriodRequest { PeriodId = periodId };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("countall")]
    public async Task<IActionResult> GetCount(CancellationToken cancellationToken)
    {
        var request = new GetDepartmentsCountRequest();
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("countdisciplines")]
    public async Task<IActionResult> GetDisciplinesCount(Guid departmentId, CancellationToken cancellationToken)
    {
        var request = new GetDepartmentDisciplinesCountRequest { DepartmentId = departmentId };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCount(Guid facultyId, CancellationToken cancellationToken)
    {
        var request = new GetDepartmentsCountOfFacultyRequest { FacultyId = facultyId };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new ExistDepartmentRequest { DepartmentId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetDepartmentByIdRequest { DepartmentId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpPut]
    public async Task<IActionResult> CreateDepartment(NewDepartmentDto department, CancellationToken cancellationToken)
    {
        var request = new CreateDepartmentRequest { Department = department };
        var response = await _mediator.Send(request, cancellationToken);
        return GetCreatedResponseResult(response);
    }

    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> UpdateDepartment(EditDepartmentDto department, CancellationToken cancellationToken)
    {
        var request = new UpdateDepartmentRequest { DepartmentToUpdate = department };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDepartment(Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteDepartmentRequest { DepartmentId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("planningitems")]
    public async Task<IActionResult> GetPlanningItems(Guid id, Guid periodId, bool onlyLoadItems = false, Guid? courseId = null, CancellationToken cancellationToken = default)
    {
        var request = new GetPlanningForDepartmentRequest
        {
            DepartmentId = id,
            PeriodId = periodId,
            CourseId = courseId,
            OnlyLoadItems = onlyLoadItems
        };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("periodstats")]
    public async Task<IActionResult> GetPeriodStatistics(Guid departmentId, Guid periodId, CancellationToken cancellationToken)
    {
        var request = new GetDepartmentStatisticsRequest
        {
            DepartmentId = departmentId,
            PeriodId = periodId
        };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }
}