using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record GetCoursesCountResponse : RequestResponseBase
{
    public int CoursesCount { get; set; }

    public override object? GetPayload() => CoursesCount;
}
