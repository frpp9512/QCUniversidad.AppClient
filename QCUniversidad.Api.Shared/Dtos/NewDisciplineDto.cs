using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos
{
    public record NewDisciplineDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
