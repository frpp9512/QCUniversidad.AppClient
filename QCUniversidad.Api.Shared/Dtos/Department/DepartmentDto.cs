using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Shared.Dtos.Department
{
    public record DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisciplinesCount { get; set; }
        public Guid FacultyId { get; set; }
        public FacultyDto Faculty { get; set; }
    }
}
