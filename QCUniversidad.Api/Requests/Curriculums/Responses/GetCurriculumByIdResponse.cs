using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Requests.Curriculums.Responses;

public record GetCurriculumByIdResponse : ResponseBase
{
    public Guid CurriculumId { get; set; }
    public CurriculumDto? Curriculum { get; set; }

    public override object? GetPayload() => Curriculum;
}
