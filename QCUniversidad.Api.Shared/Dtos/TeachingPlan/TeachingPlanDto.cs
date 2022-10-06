using QCUniversidad.Api.Shared.Dtos.Period;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.TeachingPlan;

public record TeachingPlanModel
{
    public Guid Id { get; set; }
    public PeriodDto Period { get; set; }
    public Guid PeriodId { get; set; }
    public IList<TeachingPlanItemSimpleDto> Items { get; set; }
}