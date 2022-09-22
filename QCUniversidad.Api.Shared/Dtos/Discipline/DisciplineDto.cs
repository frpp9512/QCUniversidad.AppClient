using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Shared.Dtos.Discipline
{
    public record PopulatedDisciplineDto : SimpleDisciplineDto
    {
        public int TeachersCount { get; set; }
        public int SubjectsCount { get; set; }
        public DepartmentDto Department { get; set; }
    }
}