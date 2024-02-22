using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record GetCoursesCountResponse : ResponseBase
{
    public int CoursesCount { get; set; }

    public override object? GetPayload() => CoursesCount;
}
