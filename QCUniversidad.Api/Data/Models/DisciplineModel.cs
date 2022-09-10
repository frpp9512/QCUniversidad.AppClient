using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models
{
    /// <summary>
    /// Represents a field of study, it englobes subjects, investigations and teachers.
    /// </summary>
    public record DisciplineModel
    {
        /// <summary>
        /// Primary key value.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the discipline. Example: Mathematics.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the discipline.
        /// </summary>
        public string? Description { get; set; }

        public IList<SubjectModel>? Subjects { get; set; }

        public IList<TeacherDiscipline>? DisciplineTeachers { get; set; }
    }
}