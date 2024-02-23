using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Careers.Responses;

public record ExistsCareerResponse : RequestResponseBase
{
    public Guid CareerId { get; set; }
    public bool CareerExists { get; set; }

    public override object? GetPayload() => CareerExists;
}
