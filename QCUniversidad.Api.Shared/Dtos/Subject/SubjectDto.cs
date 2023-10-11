using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Shared.Dtos.Subject;

public record SubjectDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid DisciplineId { get; set; }
    public required PopulatedDisciplineDto Discipline { get; set; }
}