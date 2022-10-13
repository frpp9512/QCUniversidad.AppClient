using QCUniversidad.Api.Shared.Dtos.Subject;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.TeachingPlan
{
    public record EditTeachingPlanItemDto : NewTeachingPlanItemDto
    {
        public Guid Id { get; set; }
    }
}
