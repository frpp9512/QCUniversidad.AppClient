using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Curriculums.Responses;

public record DeleteCurriculumRequestResponse : RequestResponseBase
{
    public bool Deleted { get; set; }

    public override object? GetPayload() => Deleted;
}
