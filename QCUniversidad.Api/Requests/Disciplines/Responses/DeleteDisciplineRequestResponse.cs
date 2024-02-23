using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Disciplines.Responses;

public record DeleteDisciplineRequestResponse : RequestResponseBase
{
    public Guid DisciplineId { get; set; }
    public bool Deleted { get; set; }

    public override object? GetPayload() => DisciplineId;
}
