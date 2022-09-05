using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos
{
    public class FacultyDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Campus { get; set; }

        public int DepartmentCount { get; set; } = 0;

        public int CareersCount { get; set; } = 0;
    }
}
