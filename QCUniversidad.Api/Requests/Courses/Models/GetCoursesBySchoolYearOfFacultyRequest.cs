using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class GetCoursesBySchoolYearOfFacultyRequest : RequestBase<GetCoursesBySchoolYearOfFacultyRequestResponse>
{
    public Guid SchoolYearId { get; set; }
    public Guid FacultyId { get; set; }
}
