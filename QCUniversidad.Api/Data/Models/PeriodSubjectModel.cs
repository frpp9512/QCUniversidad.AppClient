﻿namespace QCUniversidad.Api.Data.Models;

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
    public PeriodModel Period { get; set; }

    /// <summary>
    /// The id of the subject that will be teached in the period.
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// The subject that will be teached in the period.
    /// </summary>
    public SubjectModel Subject { get; set; }

    /// <summary>
    /// The id of the course will recieve the subject in the period.
    /// </summary>
    public Guid CourseId { get; set; }

    /// <summary>
    /// The course will recieve the subject in the period.
    /// </summary>
    public CourseModel Course { get; set; }

    /// <summary>
    /// The amount of midterms exams that the subject have in the period.
    /// </summary>
    public int MidtermExamsCount { get; set; }

    /// <summary>
    /// Defines if the subject have final exam in the period.
    /// </summary>
    public bool HaveFinalExam { get; set; }
}