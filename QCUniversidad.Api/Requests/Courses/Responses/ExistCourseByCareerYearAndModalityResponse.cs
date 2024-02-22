using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record ExistCourseByCareerYearAndModalityResponse : ResponseBase
{
    public Guid CareerId { get; set; }
    public int CareerYear { get; set; }
    public TeachingModality TeachingModality { get; set; }
    public bool Exists { get; set; }

    public override object? GetPayload() => Exists;
}
