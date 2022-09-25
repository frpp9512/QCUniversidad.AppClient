using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Curriculum
{
    public record EditCurriculumDto : NewCurriculumDto
    {
        public Guid Id { get; set; }
    }
}
