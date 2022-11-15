using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Shared.Dtos.Career;

public record CareerDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool PostgraduateCourse { get; set; }
    public Guid FacultyId { get; set; }
    public FacultyDto? Faculty { get; set; }
    public IList<SimpleDepartmentDto>? Departments { get; set; }
}