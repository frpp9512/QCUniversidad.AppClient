using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.Api.Shared.Dtos.LoadItem;

namespace QCUniversidad.Api.Shared.Dtos.Teacher;

public record TeacherDto : EditTeacherDto
{
    public DateTime? Birthday { get; set; }
    public int Age { get; set; }
    public DepartmentDto? Department { get; set; }
    public IList<PopulatedDisciplineDto>? Disciplines { get; set; }
    public TeacherLoadDto? Load { get; set; }
    public IList<LoadViewItemDto>? LoadViewItems { get; set; }
}