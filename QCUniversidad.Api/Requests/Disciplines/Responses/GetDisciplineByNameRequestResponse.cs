using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Requests.Disciplines.Responses;

public record GetDisciplineByNameRequestResponse : RequestResponseBase
{
    public string? DisciplineName { get; set; }
    public PopulatedDisciplineDto? Discipline { get; set; }

    public override object? GetPayload() => Discipline;
}
