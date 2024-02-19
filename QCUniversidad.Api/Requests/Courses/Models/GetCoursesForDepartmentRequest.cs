using MediatR;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class GetCoursesForDepartmentRequest : IRequest<GetCoursesForDepartmentResponse>
{
    public required Guid DepartmentId { get; set; }
    public Guid? SchoolYearId { get; set; }
}
