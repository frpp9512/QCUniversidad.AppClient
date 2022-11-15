namespace QCUniversidad.Api.Shared.Dtos.SchoolYear;

public record SimpleSchoolYearDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool Current { get; set; }
}
