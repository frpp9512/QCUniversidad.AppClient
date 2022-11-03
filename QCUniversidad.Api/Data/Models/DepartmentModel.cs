using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models
{
    /// <summary>
    /// Represents a stucture of teachers.
    /// </summary>
    public record DepartmentModel
    {
        /// <summary>
        /// Primary key value.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// The name of the department. Example: Ingeniería Industrial.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the department.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The internat management identifier. Example: 2460
        /// </summary>
        public string InternalId { get; set; }

        /// <summary>
        /// The disciplines managed by the department.
        /// </summary>
        public IList<DisciplineModel> Disciplines { get; set; }

        /// <summary>
        /// The teachers of the department.
        /// </summary>
        public IList<TeacherModel> Teachers { get; set; }

        /// <summary>
        /// The id of the faculty where the department belongs to.
        /// </summary>
        public Guid FacultyId { get; init; }

        /// <summary>
        /// The faculty where the department belongs to.
        /// </summary>
        public FacultyModel Faculty { get; init; }

        /// <summary>
        /// The relation of the careers attended by the department.
        /// </summary>
        public IList<DepartmentCareer> DepartmentCareers { get; set; }
    }
}