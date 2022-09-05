using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models
{
    /// <summary>
    /// Represents the relationship between the Curriculum and the Subjects.
    /// </summary>
    public record CurriculumSubject
    {
        public Guid CurriculumId { get; set; }
        public CurriculumModel Curriculum { get; set; }
        public Guid SubjectId { get; set; }
        public SubjectModel Subject { get; set; }

        /// <summary>
        /// The ordinal year number of the carrer when the subject should be teached. Example: 3 (3rd year)
        /// </summary>
        public int CareerYear { get; set; }
    }
}
