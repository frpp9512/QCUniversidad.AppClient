using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Curriculums.Responses;

public record DeleteCurriculumResponse : RequestResponseBase
{
    public bool Deleted { get; set; }

    public override object? GetPayload() => Deleted;
}
