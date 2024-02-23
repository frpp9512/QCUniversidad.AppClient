using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Careers.Responses;

public record GetCareersCountRequestResponse : RequestResponseBase
{
    public int CareersCount { get; set; }

    public override object? GetPayload() => CareersCount;
}
