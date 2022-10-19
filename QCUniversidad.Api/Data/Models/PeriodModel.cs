using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models
{
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
        /// The ordinal number of the period. Example: 1 (for the 1st period)
        /// </summary>
        public int OrderNumber { get; set; }

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
        /// The enrolment planned for the period.
        /// </summary>
        public uint Enrolment { get; set; }

        /// <summary>
        /// The set of teaching items.
        /// </summary>
        public IList<TeachingPlanItemModel> TeachingPlan { get; set; }

        /// <summary>
        /// The id of the shool year when the period passes.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// The shool year when the period passes.
        /// </summary>
        public CourseModel Course { get; set; }
    }
}