using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models
{
    /// <summary>
    /// Represents the relationship between the Curriculum and the Discipline.
    /// </summary>
    public record CurriculumDiscipline
    {
        public Guid CurriculumId { get; set; }
        public CurriculumModel Curriculum { get; set; }
        public Guid DisciplineId { get; set; }
        public DisciplineModel Discipline { get; set; }
    }
}
