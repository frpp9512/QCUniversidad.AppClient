using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Career
{
    public record EditCareerDto : NewCareerDto
    {
        public Guid Id { get; set; }
    }
}
