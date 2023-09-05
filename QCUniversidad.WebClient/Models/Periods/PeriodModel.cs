using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Models.SchoolYears;

namespace QCUniversidad.WebClient.Models.Periods;

/// <summary>
/// Represents a period inside the school year, when the students will recieve the subjects.
/// </summary>
public record PeriodModel
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Starts { get; set; }
    public DateTimeOffset Ends { get; set; }
    public double MonthsCount { get; set; }
    public double TimeFund { get; set; }
    public Guid SchoolYearId { get; set; }
    public required SchoolYearModel SchoolYear { get; set; }
    public IList<TeachingPlanItemModel>? PlanItems { get; set; }

    public override string ToString() => $"{Starts:dd-MM-yyyy} - {Ends:dd-MM-yyyy}";
}