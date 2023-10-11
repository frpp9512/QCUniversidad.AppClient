namespace QCUniversidad.Api.Shared.Dtos.Department;

public record NewDepartmentDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsStudyCenter { get; set; }
    public required string InternalId { get; set; }
    public Guid FacultyId { get; set; }
    public Guid[]? SelectedCareers { get; set; }
}