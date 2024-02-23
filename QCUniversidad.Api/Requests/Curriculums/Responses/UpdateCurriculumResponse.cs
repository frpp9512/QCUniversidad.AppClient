using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Curriculums.Responses;

public record UpdateCurriculumResponse : RequestResponseBase
{
    public bool Updated { get; set; }

    public override object? GetPayload()
    {
        return Updated;
    }
}
