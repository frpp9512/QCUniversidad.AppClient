namespace QCUniversidad.Api.Shared.Dtos.Department;

public record NewDepartmentDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsStudyCenter { get; set; }
    public string InternalId { get; set; }
    public Guid FacultyId { get; set; }
    public Guid[]? SelectedCareers { get; set; }
}