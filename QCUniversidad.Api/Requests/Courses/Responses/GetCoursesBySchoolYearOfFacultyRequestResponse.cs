using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record GetCoursesBySchoolYearOfFacultyRequestResponse : RequestResponseBase
{
    public Guid SchoolYearId { get; set; }
    public Guid FacultyId { get; set; }
    public List<CourseDto>? Courses { get; set; }

    public override object? GetPayload() => Courses;
}
