using QCUniversidad.Api.Shared.Dtos.Subject;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.TeachingPlan
{
    public record NewTeachingPlanItemDto
    {
        public Guid SubjectId { get; set; }
        public Guid CourseId { get; set; }
        public TeachingActivityType Type { get; set; }
        public double HoursPlanned { get; set; }
        public uint GroupsAmount { get; set; }
        public double TotalHoursPlanned { get; set; }
        public Guid PeriodId { get; set; }
    }
}
