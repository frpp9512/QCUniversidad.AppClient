using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record DeleteCourseResponse : ResponseBase
{
    public bool Deleted { get; set; }

    public override object GetPayload() => new { Deleted };
}
