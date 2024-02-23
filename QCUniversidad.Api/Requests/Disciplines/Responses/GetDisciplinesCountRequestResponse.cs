using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Disciplines.Responses;

public record GetDisciplinesCountRequestResponse : RequestResponseBase
{
    public int Count { get; set; }

    public override object? GetPayload() => Count;
}
