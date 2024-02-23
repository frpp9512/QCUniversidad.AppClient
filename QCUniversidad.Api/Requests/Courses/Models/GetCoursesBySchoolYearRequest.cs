using MediatR;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class GetCoursesBySchoolYearRequest : IRequest<GetCoursesBySchoolYearRequestResponse>
{
    public Guid SchoolYearId { get; set; }
}
