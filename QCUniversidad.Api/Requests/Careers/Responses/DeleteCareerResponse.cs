using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Careers.Responses;

public record DeleteCareerResponse : ResponseBase
{
    public Guid CareerId { get; set; }
    public bool Deleted { get; set; }

    public override object? GetPayload() => Deleted;
}
