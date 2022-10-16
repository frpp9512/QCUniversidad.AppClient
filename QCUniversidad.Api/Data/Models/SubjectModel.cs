using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models
{
    /// <summary>
    /// Represents a subject that will be teached.
    /// </summary>
    public record SubjectModel
    {
        /// <summary>
        /// Primary key value.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The subject name. Example: Statistics II.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the subject.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The id of the discipline which the subject belongs.
        /// </summary>
        public Guid DisciplineId { get; set; }

        /// <summary>
        /// The discipline which the subject belongs.
        /// </summary>
        public DisciplineModel Discipline { get; set; }

        /// <summary>
        /// Defines if the subject is active for planning and load distribution.
        /// </summary>
        public bool Active { get; set; }
    }
}