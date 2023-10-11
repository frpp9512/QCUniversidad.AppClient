using QCUniversidad.Api.Shared.Dtos.Career;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Shared.Dtos.Curriculum;

public record CurriculumDto
{
    public Guid Id { get; set; }
    public required string Denomination { get; set; }
    public string? Description { get; set; }
    public int SubjectsCount { get; set; }
    public Guid CareerId { get; set; }
    public required CareerDto Career { get; set; }
    public required IList<SimpleDisciplineDto> CurriculumDisciplines { get; set; }
}
