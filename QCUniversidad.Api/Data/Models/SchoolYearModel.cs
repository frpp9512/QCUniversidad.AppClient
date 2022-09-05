using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models
{
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
        public int CareerYear { get; set; }

        /// <summary>
        /// The denomination of the school year. Example: 2021-2022.
        /// </summary>
        public string Denomination { get; set; }

        /// <summary>
        /// The date when the school year begins.
        /// </summary>
        public DateTimeOffset Starts { get; set; }

        /// <summary>
        /// The date when the school year ends.
        /// </summary>
        public DateTimeOffset Ends { get; set; }

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
        public CareerModel Career { get; set; }

        /// <summary>
        /// The id of the curriculum that will be taught in the year.
        /// </summary>
        public Guid CurriculumId { get; set; }

        /// <summary>
        /// The id of the curriculum that will be taught in the year.
        /// </summary>
        public CurriculumModel Curriculum { get; set; }

        /// <summary>
        /// The set of periods of the shool year.
        /// </summary>
        public IList<PeriodModel> Periods { get; set; }
    }

    /// <summary>
    /// The teaching modality that the students use for the career.
    /// </summary>
    public enum TeachingModality
    {
        /// <summary>
        /// When the students are everyday in the campus reciveing the subjects.
        /// </summary>
        Classroom,

        /// <summary>
        /// When the students recieve the subjects in specific dates in the months, the self study is the key.
        /// </summary>
        ByMeeting,

        /// <summary>
        /// When the students recieve the subjects via internet, and only make presence for the tests.
        /// </summary>
        DistanceLearning
    }
}