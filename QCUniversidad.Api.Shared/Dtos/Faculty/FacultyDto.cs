namespace QCUniversidad.Api.Shared.Dtos.Faculty;

public record FacultyDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Campus { get; set; }
    public required string InternalId { get; set; }
    public int DepartmentCount { get; set; } = 0;
    public int CareersCount { get; set; } = 0;
}