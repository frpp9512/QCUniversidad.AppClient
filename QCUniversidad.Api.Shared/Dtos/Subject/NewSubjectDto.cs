namespace QCUniversidad.Api.Shared.Dtos.Subject;

public record NewSubjectDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid DisciplineId { get; set; }
}
