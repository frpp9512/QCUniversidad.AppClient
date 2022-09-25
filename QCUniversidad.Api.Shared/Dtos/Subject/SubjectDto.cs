using QCUniversidad.Api.Shared.Dtos.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Subject
{
    public record SubjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid DisciplineId { get; set; }
        public PopulatedDisciplineDto Discipline { get; set; }
    }
}