using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Data.Models;

public record PeriodSubjectModel
{
    /// <summary>
    /// Primary key value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The id of the period when the subject will be teached.
    /// </summary>
    public Guid PeriodId { get; set; }

    /// <summary>
    /// The period when the subject will be teached.
    /// </summary>
    public required PeriodModel Period { get; set; }

    /// <summary>
    /// The id of the subject that will be teached in the period.
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// The subject that will be teached in the period.
    /// </summary>
    public required SubjectModel Subject { get; set; }

    /// <summary>
    /// The id of the course will recieve the subject in the period.
    /// </summary>
    public Guid CourseId { get; set; }

    /// <summary>
    /// The course will recieve the subject in the period.
    /// </summary>
    public required CourseModel Course { get; set; }

    /// <summary>
    /// The amount of midterms exams that the subject have in the period.
    /// </summary>
    public int MidtermExamsCount { get; set; }

    /// <summary>
    /// Defines how the subject will be terminated.
    /// </summary>
    public SubjectTerminationMode TerminationMode { get; set; }

    /// <summary>
    /// The amount of class hours planned in the period for the subject.
    /// </summary>
    public double HoursPlanned { get; set; }

    /// <summary>
    /// The amount of time (in hours) planned for the subject in the period.
    /// </summary>
    public double TotalHours { get; set; }
}