using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.LoadItem
{
    public record LoadItemDto : EditLoadItemDto
    {
        public TeachingPlanItemSimpleDto PlanningItem { get; set; }
        public TeacherDto Teacher { get; set; }
    }
}
