using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Requests.Curriculums.Responses;

public record GetCurriculumsRangeResponse : ResponseBase
{
    public int From { get; set; }
    public int To { get; set; }
    public List<CurriculumDto>? Curriculums { get; set; }

    public override object? GetPayload() => Curriculums;
}
