using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Data.Models;

/// <summary>
/// A set of periods where will be taught a set of subjects.
/// </summary>
public record CourseModel
{
    /// <summary>
    /// Primary key value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The id of the school year when the course take place.
    /// </summary>
    public Guid SchoolYearId { get; set; }

    /// <summary>
    /// The school year when the course take place.
    /// </summary>
    public SchoolYearModel? SchoolYear { get; set; }

    /// <summary>
    /// The ordinal year number for the carrer. Example: 3 (3rd year)
    /// </summary>
    public int CareerYear { get; set; }

    /// <summary>
    /// The denomination of the school year. Example: 2021-2022.
    /// </summary>
    public string? Denomination { get; set; }

    /// <summary>
    /// The modality in which the students will study.
    /// </summary>
    public TeachingModality TeachingModality { get; set; }

    /// <summary>
    /// The enrolment planned for the period.
    /// </summary>
    public uint Enrolment { get; set; }

    /// <summary>
    /// The id of the carrer coursed by the students in the year.
    /// </summary>
    public Guid CareerId { get; set; }

    /// <summary>
    /// The carrer coursed by the students in the year.
    /// </summary>
    public CareerModel? Career { get; set; }

    /// <summary>
    /// The id of the curriculum that will be taught in the year.
    /// </summary>
    public Guid CurriculumId { get; set; }

    /// <summary>
    /// The id of the curriculum that will be taught in the year.
    /// </summary>
    public CurriculumModel? Curriculum { get; set; }

    /// <summary>
    /// The set of plan items associated to the course.
    /// </summary>
    public IList<TeachingPlanItemModel>? PlanItems { get; set; }
}