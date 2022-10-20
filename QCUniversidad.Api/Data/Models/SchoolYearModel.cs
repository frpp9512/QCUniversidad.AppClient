using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models
{
    public record SchoolYearModel
    {
        /// <summary>
        /// The primary key value.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the school year. Ex. 2022-2023
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the school year.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Defines if is the current school year.
        /// </summary>
        public bool Current { get; set; }

        /// <summary>
        /// The period of the current school year.
        /// </summary>
        public IList<PeriodModel> Periods { get; set; }

        /// <summary>
        /// The set of courses teached in the school year.
        /// </summary>
        public IList<CourseModel> Courses { get; set; }
    }
}