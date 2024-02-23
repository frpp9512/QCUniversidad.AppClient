using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Requests.Disciplines.Responses;

public record GetDisciplineByIdRequestResponse : RequestResponseBase
{
    public Guid DisciplineId { get; set; }
    public PopulatedDisciplineDto? Discipline { get; set; }

    public override object? GetPayload() => Discipline;
}
