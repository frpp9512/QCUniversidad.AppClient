using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record GetCoursesBySchoolYearRequestResponse : RequestResponseBase
{
    public Guid SchoolYearId { get; set; }
    public List<CourseDto>? Courses { get; set; }

    public override object? GetPayload() => Courses;
}
