using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models
{
    /// <summary>
    /// The plan of study that will be taught to the students of the year.
    /// </summary>
    public record CurriculumModel
    {
        /// <summary>
        /// Primary key value.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The denomination of the curriculum. Example: E
        /// </summary>
        public string Denomination { get; set; }

        /// <summary>
        /// The description of the curriculum. Example: A plan deepen in industrial engineering topics such as the study of time.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The id of the career of the curriculum
        /// </summary>
        public Guid CareerId { get; set; }

        /// <summary>
        /// The career of the curriculum
        /// </summary>
        public CareerModel Career { get; set; }

        /// <summary>
        /// The set of subjects covered in the curriculum
        /// </summary>
        public IList<CurriculumDiscipline> CurriculumDisciplines { get; set; }
    }
}