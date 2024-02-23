using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Responses;

public record UpdateCareerResponse : RequestResponseBase
{
    public CareerDto? CareerUpdated { get; set; }

    public override object? GetPayload() => CareerUpdated;
}
