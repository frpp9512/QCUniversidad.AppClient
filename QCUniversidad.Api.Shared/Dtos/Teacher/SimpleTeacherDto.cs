using QCUniversidad.Api.Shared.Dtos.Department;
using System.ComponentModel.DataAnnotations.Schema;

namespace QCUniversidad.Api.Shared.Dtos.Teacher;

public record SimpleTeacherDto : EditTeacherDto
{
    [NotMapped]
    public DateTime? Birthday { get; set; }
    public DepartmentDto? Department { get; set; }
    public TeacherLoadDto? Load { get; set; }
}
