using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Curriculums.Responses;

public record GetCurriculumsCountResponse : ResponseBase
{
    public int CurriculumsCount { get; set; }

    public override object? GetPayload() => CurriculumsCount;
}
