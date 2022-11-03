using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Shared.Dtos.Career;

public record CareerDto : EditCareerDto
{
    public FacultyDto? Faculty { get; set; }
    public IList<SimpleDepartmentDto>? Departments { get; set; }
}