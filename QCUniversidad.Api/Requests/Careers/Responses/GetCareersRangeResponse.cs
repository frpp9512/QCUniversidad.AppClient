using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Responses;

public record GetCareersRangeResponse : ResponseBase
{
    public int From { get; set; }
    public int To { get; set; }
    public List<CareerDto>? Careers { get; set; }

    public override object? GetPayload() => Careers;
}
