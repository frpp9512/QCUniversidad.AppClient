using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record GetCourseByIdResponse : ResponseBase
{
    public Guid CourseId { get; set; }
    public CourseDto? Course { get; set; }

    public override object? GetPayload() => Course;
}
