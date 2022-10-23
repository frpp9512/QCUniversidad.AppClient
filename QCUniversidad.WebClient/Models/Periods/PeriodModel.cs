using QCUniversidad.WebClient.Models.Courses;
using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Models.SchoolYears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public SchoolYearModel SchoolYear { get; set; }
    public IList<TeachingPlanItemModel>? PlanItems { get; set; }

    public override string ToString() => $"{Starts.ToString("dd-MM-yyyy")} - {Ends.ToString("dd-MM-yyyy")}";
}