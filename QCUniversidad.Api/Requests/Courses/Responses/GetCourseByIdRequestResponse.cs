using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record GetCourseByIdRequestResponse : RequestResponseBase
{
    public Guid CourseId { get; set; }
    public CourseDto? Course { get; set; }

    public override object? GetPayload() => Course;
}
