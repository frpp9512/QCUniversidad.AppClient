using QCUniversidad.Api.Shared.Dtos.LoadItem;
using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.Api.Shared.Dtos.Subject;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.TeachingPlan;

public record TeachingPlanItemDto
{
    public Guid Id { get; set; }
    public Guid SubjectId { get; set; }
    public SubjectDto? Subject { get; set; }
    public TeachingActivityType Type { get; set; }
    public double HoursPlanned { get; set; }
    public uint GroupsAmount { get; set; }
    public double TotalHoursPlanned { get; set; }
    public bool FromPostgraduateCourse { get; set; }
    public double TotalLoadCovered { get; set; }
    public double LoadCoveredPercent => Math.Round((TotalLoadCovered / TotalHoursPlanned) * 100, 1);
    public bool AllowLoad { get; set; }
    public Guid PeriodId { get; set; }
    public PeriodDto TeachingPlan { get; set; }
    public IList<LoadItemDto> LoadItems { get; set; }
}