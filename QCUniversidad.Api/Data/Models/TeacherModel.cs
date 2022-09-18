using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models
{
    /// <summary>
    /// Represents a person that teach a set of subjects from a determined discipline.
    /// </summary>
    public record TeacherModel
    {
        /// <summary>
        /// Primary key value.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The full name of the teacher.
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// The identity card number.
        /// </summary>
        [MaxLength(11), MinLength(11)]
        public string? PersonalId { get; set; }

        /// <summary>
        /// The position that occupies in the university.
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// Defines if the teacher provides services for other faculties.
        /// </summary>
        public bool ServiceProvider { get; set; }

        /// <summary>
        /// The disciplines whose the teacher can teach.
        /// </summary>
        public IList<TeacherDiscipline> TeacherDisciplines { get; set; }
    }
}