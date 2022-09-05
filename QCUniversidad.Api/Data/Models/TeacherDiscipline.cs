using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models
{
    /// <summary>
    /// Represents the relationship between a teacher and a discipline.
    /// </summary>
    public class TeacherDiscipline
    {
        public Guid TeacherId { get; set; }
        public TeacherModel Teacher { get; set; }
        public Guid DisciplineId { get; set; }
        public DisciplineModel Discipline { get; set; }
    }
}
