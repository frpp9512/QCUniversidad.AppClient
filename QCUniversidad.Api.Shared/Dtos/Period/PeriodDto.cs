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
    public required SchoolYearDto SchoolYear { get; set; }
    public IList<TeachingPlanItemSimpleDto>? PlanItems { get; set; }
}