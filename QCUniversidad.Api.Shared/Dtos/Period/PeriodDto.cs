using QCUniversidad.Api.Shared.Dtos.Career;
using QCUniversidad.Api.Shared.Dtos.Curriculum;
using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCUniversidad.Api.Shared.Dtos.SchoolYear;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;

namespace QCUniversidad.Api.Shared.Dtos.Period;

public record PeriodDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Starts { get; set; }
    public DateTimeOffset Ends { get; set; }
    public double MonthsCount { get; set; }
    public double TimeFund { get; set; }
    public Guid SchoolYearId { get; set; }
    public SchoolYearDto SchoolYear { get; set; }
    public IList<TeachingPlanItemSimpleDto>? PlanItems { get; set; }
}