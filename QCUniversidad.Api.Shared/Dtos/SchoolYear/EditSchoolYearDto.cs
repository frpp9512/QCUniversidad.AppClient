using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.SchoolYear
{
    public record EditSchoolYearDto : NewSchoolYearDto
    {
        public Guid Id { get; set; }
    }
}
