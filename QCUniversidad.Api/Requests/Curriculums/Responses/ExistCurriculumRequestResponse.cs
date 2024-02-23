using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Curriculums.Responses;

public record ExistCurriculumRequestResponse : RequestResponseBase
{
    public bool ExistCurriculum { get; set; }

    public override object? GetPayload() => ExistCurriculum;
}
