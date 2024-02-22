using MediatR;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class GetCoursesBySchoolYearRequest : IRequest<GetCoursesBySchoolYearResponse>
{
    public Guid SchoolYearId { get; set; }
}
