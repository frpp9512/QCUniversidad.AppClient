using MediatR;
using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.SchoolYears.Models;
using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseController(IMediator mediator) : ApiControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync(int from = 0, int to = 0, CancellationToken cancellationToken = default)
    {
        var request = new GetCoursesRangeRequest { From = from, To = to };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("listbyschoolyear")]
    public async Task<IActionResult> GetListBySchoolYearAsync(Guid schoolYearId, CancellationToken cancellationToken)
    {
        var request = new GetCoursesBySchoolYearRequest { SchoolYearId = schoolYearId };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("listbyschoolyearandfaculty")]
    public async Task<IActionResult> GetListBySchoolYearAndFacultyAsync(Guid schoolYearId, Guid facultyId, CancellationToken cancellationToken)
    {
        var request = new GetCoursesBySchoolYearOfFacultyRequest
        {
            SchoolYearId = schoolYearId,
            FacultyId = facultyId
        };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("listbycareerschoolyearandfaculty")]
    public async Task<IActionResult> GetListByCareerSchoolYearAndFacultyAsync(Guid careerId, Guid schoolYearId, Guid facultyId, CancellationToken cancellationToken)
    {
        var request = new GetCoursesBySchoolYearAndCareerOfFacultyRequest
        {
            SchoolYearId = schoolYearId,
            FacultyId = facultyId,
            CareerId = careerId
        };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCount(CancellationToken cancellationToken)
    {
        var request = new GetCoursesCountRequest();
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("periodscount")]
    public async Task<IActionResult> GetPeriodsCount(Guid schoolYearId, CancellationToken cancellationToken)
    {
        var request = new GetSchoolYearPeriodsCountRequest { SchoolYearId = schoolYearId };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("exists")]
    public async Task<IActionResult> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new ExistCourseRequest { CourseId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("existsbycareeryearandmodality")]
    public async Task<IActionResult> ExistsByCareerYearAndModality(Guid careerId, int careerYear, int modality, CancellationToken cancellationToken)
    {
        var request = new ExistCourseByCareerYearAndModalityRequest
        {
            CareerId = careerId,
            CareerYear = careerYear,
            Modality = (TeachingModality)modality
        };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpPut]
    public async Task<IActionResult> CreateAsync(NewCourseDto course, CancellationToken cancellationToken)
    {
        if (course is null)
        {
            return BadRequest("The discipline cannot be null.");
        }

        var request = new CreateCourseRequest { NewCourse = course };
        var response = await _mediator.Send(request, cancellationToken);
        return GetCreatedResponseResult(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetCourseByIdRequest { CourseId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(EditCourseDto course, CancellationToken cancellationToken)
    {
        if (course is null)
        {
            return BadRequest("The school year cannot be null.");
        }

        var request = new UpdateCourseRequest { CourseToUpdate = course };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteCourseRequest { CourseId = id };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("listfordepartment")]
    public async Task<IActionResult> GetCoursesForDepartment(Guid departmentId, Guid? schoolYearId = null, CancellationToken cancellationToken = default)
    {
        var request = new GetCoursesForDepartmentRequest { DepartmentId = departmentId, SchoolYearId = schoolYearId };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }

    [HttpGet]
    [Route("periodplanninginfo")]
    public async Task<IActionResult> GetPeriodPlanningInfoAsync(Guid id, Guid periodId, CancellationToken cancellationToken)
    {
        var request = new GetCoursePlanningInfoForPeriodRequest { CourseId = id, PeriodId = periodId };
        var response = await _mediator.Send(request, cancellationToken);
        return GetResponseResult(response);
    }
}