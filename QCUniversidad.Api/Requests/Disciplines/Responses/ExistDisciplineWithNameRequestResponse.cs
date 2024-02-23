using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Disciplines.Responses;

public record ExistDisciplineWithNameRequestResponse : RequestResponseBase
{
    public string? Name { get; set; }
    public bool Exist { get; set; }

    public override object? GetPayload() => Exist;
}
