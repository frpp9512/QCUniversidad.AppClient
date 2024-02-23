using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Careers.Responses;

public record DeleteCareerRequestResponse : RequestResponseBase
{
    public Guid CareerId { get; set; }
    public bool Deleted { get; set; }

    public override object? GetPayload() => Deleted;
}
