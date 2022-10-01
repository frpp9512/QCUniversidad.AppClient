using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Curriculums;
using QCUniversidad.WebClient.Models.Periods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.SchoolYears;

/// <summary>
/// A set of periods where will be taught a set of subjects.
/// </summary>
public record SchoolYearModel
{
    /// <summary>
    /// Primary key value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The ordinal year number for the carrer. Example: 3 (3rd year)
    /// </summary>
    public int CareerYear { get; set; } = 1;

    /// <summary>
    /// The denomination of the school year. Example: 2021-2022.
    /// </summary>
    public string Denomination { get; set; }

    /// <summary>
    /// The date when the school year begins.
    /// </summary>
    public DateTimeOffset Starts { get; set; } = DateTime.Now;

    /// <summary>
    /// The date when the school year ends.
    /// </summary>
    public DateTimeOffset Ends { get; set; } = DateTime.Now.AddMonths(9);

    /// <summary>
    /// The modality in which the students will study.
    /// </summary>
    public TeachingModality TeachingModality { get; set; }

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
    /// The set of periods of the shool year.
    /// </summary>
    public IList<PeriodModel>? Periods { get; set; }
}