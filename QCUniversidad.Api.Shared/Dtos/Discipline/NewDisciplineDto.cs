namespace QCUniversidad.Api.Shared.Dtos.Discipline;

public record NewDisciplineDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid DepartmentId { get; set; }
}
