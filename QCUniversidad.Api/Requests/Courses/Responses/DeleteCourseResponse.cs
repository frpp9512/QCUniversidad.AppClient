using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record DeleteCourseResponse : RequestResponseBase
{
    public bool Deleted { get; set; }

    public override object GetPayload() => new { Deleted };
}
