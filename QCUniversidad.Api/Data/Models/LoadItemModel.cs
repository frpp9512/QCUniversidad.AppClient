namespace QCUniversidad.Api.Data.Models;

public record LoadItemModel
{
    /// <summary>
    /// Primary key value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The id of planning item related.
    /// </summary>
    public Guid PlanningItemId { get; set; }

    /// <summary>
    /// The planning item related.
    /// </summary>
    public required TeachingPlanItemModel PlanningItem { get; set; }

    /// <summary>
    /// The id of the teacher assigned to the load.
    /// </summary>
    public Guid TeacherId { get; set; }

    /// <summary>
    /// The teacher assigned to the load.
    /// </summary>
    public required TeacherModel Teacher { get; set; }

    /// <summary>
    /// The amount of hours the teacher covers for the plan item.
    /// </summary>
    public double HoursCovered { get; set; }
}
