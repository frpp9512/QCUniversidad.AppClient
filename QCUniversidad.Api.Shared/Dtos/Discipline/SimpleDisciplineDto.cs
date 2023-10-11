namespace QCUniversidad.Api.Shared.Dtos.Discipline;

public record SimpleDisciplineDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid DepartmentId { get; set; }
}
