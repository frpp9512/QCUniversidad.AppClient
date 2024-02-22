using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record UpdateCourseResponse : ResponseBase
{
    public EditCourseDto? UpdatedCourse { get; set; }

    public override object? GetPayload() => UpdatedCourse;
}
