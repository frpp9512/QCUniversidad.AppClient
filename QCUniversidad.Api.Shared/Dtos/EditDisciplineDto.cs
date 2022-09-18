using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos
{
    public record EditDisciplineDto : NewDisciplineDto
    {
        public Guid Id { get; set; }
    }
}
