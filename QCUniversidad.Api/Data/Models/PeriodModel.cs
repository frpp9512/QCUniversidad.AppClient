namespace QCUniversidad.Api.Data.Models;

/// <summary>
/// Represents a period inside the school year, when the students will recieve the subjects.
/// </summary>
public record PeriodModel
{
    /// <summary>
    /// Primary key value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The description of the period.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The date when the period starts.
    /// </summary>
    public DateTimeOffset Starts { get; set; }

    /// <summary>
    /// The date when the period ends.
    /// </summary>
    public DateTimeOffset Ends { get; set; }

    /// <summary>
    /// The amount of months covered in the period.
    /// </summary>
    public double MonthsCount { get; set; }

    /// <summary>
    /// The time fund for the teachers in the period.
    /// </summary>
    public double TimeFund { get; set; }

    /// <summary>
    /// The set of teaching items.
    /// </summary>
    public IList<TeachingPlanItemModel>? TeachingPlan { get; set; }

    /// <summary>
    /// The non-teaching load of the teachers in the period.
    /// </summary>
    public IList<NonTeachingLoadModel>? NonTeachingLoad { get; set; }

    /// <summary>
    /// The id of the shool year when the period occurs.
    /// </summary>
    public Guid SchoolYearId { get; set; }

    /// <summary>
    /// The shool year when the period occurs.
    /// </summary>
    public SchoolYearModel? SchoolYear { get; set; }

    /// <summary>
    /// The set of subjects teached in the period.
    /// </summary>
    public IList<PeriodSubjectModel> PeriodSubjects { get; set; }

    public override string ToString() => $"{Starts.ToString("dd-MM-yyyy")} - {Ends.ToString("dd-MM-yyyy")}";
}