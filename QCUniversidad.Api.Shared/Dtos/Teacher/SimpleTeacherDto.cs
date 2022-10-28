using QCUniversidad.Api.Shared.Dtos.Department;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Teacher;

public record SimpleTeacherDto : EditTeacherDto
{
    [NotMapped]
    public DateTime? Birthday { get; set; }
    public DepartmentDto? Department { get; set; }
    public TeacherLoadDto? Load { get; set; }
}
