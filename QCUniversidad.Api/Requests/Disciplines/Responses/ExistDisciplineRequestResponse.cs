using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Disciplines.Responses;

public record ExistDisciplineRequestResponse : RequestResponseBase
{
    public Guid DisciplineId { get; set; }
    public bool Exist { get; set; }

    public override object? GetPayload() => Exist;
}
