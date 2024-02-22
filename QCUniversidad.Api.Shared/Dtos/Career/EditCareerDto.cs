namespace QCUniversidad.Api.Shared.Dtos.Career;

public record EditCareerDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool PostgraduateCourse { get; set; }
    public Guid FacultyId { get; set; }
}
