using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Requests.Disciplines.Responses;

public record GetDisciplinesRangeRequestResponse : RequestResponseBase
{
    public int From { get; set; }
    public int To { get; set; }
    public List<PopulatedDisciplineDto>? Disciplines { get; set; }

    public override object? GetPayload() => Disciplines;
}
