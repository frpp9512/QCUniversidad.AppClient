using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Shared.Dtos.Teacher
{
    public record TeacherDto : EditTeacherDto
    {
        [NotMapped]
        public DateTime? Birthday { get; set; }
        public DepartmentDto Department { get; set; }
        public IList<PopulatedDisciplineDto>? Disciplines { get; set; }
        public TeacherLoadDto? Load { get; set; }
    }
}