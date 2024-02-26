using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class GetCoursesBySchoolYearRequest : RequestBase<GetCoursesBySchoolYearRequestResponse>
{
    public Guid SchoolYearId { get; set; }
}
