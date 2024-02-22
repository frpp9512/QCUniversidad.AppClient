using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Shared.Dtos.Discipline;

public record PopulatedDisciplineDto : SimpleDisciplineDto
{
    public int TeachersCount { get; set; }
    public int SubjectsCount { get; set; }
    public required DepartmentDto Department { get; set; }
}