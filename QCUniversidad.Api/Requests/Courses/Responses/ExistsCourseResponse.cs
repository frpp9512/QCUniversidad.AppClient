using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record ExistsCourseResponse : ResponseBase
{
    public Guid CourseId { get; set; }
    public bool ExistCourse { get; set; }

    public override object? GetPayload() => ExistCourse;
}
