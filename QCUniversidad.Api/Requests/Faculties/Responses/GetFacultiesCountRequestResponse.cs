using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Faculties.Responses;

public record GetFacultiesCountRequestResponse : RequestResponseBase
{
    public int Count { get; set; }

    public override object? GetPayload() => Count;
}
