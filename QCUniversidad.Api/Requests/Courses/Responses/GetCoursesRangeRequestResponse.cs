using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record GetCoursesRangeRequestResponse : RequestResponseBase
{
    public int From { get; set; }
    public int To { get; set; }
    public List<CourseDto>? CoursesRange { get; set; }

    public override object? GetPayload() => CoursesRange;
}
