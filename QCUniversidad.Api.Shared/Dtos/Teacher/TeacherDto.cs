using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Shared.Dtos.Teacher
{
    public record TeacherDto : EditTeacherDto
    {
        public DepartmentDto Department { get; set; }
        public IList<PopulatedDisciplineDto>? Disciplines { get; set; }
    }
}
