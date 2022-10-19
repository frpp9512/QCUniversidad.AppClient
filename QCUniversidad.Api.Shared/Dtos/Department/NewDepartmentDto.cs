using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Department
{
    public record NewDepartmentDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string InternalId { get; set; }
        public Guid FacultyId { get; set; }
    }
}
