using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models
{
    /// <summary>
    /// A career managed by the faculty.
    /// </summary>
    public record CareerModel
    {
        /// <summary>
        /// Primary key value.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the career. Example: Industrial Engineering.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the career. Example: Industrial Engineering is an engineering profession that is concerned with the optimization of complex processes, systems, or organizations by developing, improving and implementing integrated systems of people, money, knowledge, information and equipment.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The identifier of the Faculty that manages the career.
        /// </summary>
        public Guid FacultyId { get; set; }

        /// <summary>
        /// The faculty that manages the career.
        /// </summary>
        public FacultyModel Faculty { get; set; }
    }
}