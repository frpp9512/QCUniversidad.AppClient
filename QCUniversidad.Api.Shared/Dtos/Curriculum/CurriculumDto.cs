using QCUniversidad.Api.Shared.Dtos.Career;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.Api.Shared.Dtos.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Curriculum
{
    public record CurriculumDto
    {
        public Guid Id { get; set; }
        public string Denomination { get; set; }
        public string? Description { get; set; }
        public int SubjectsCount { get; set; }
        public Guid CareerId { get; set; }
        public CareerDto Career { get; set; }
        public IList<SimpleDisciplineDto> CurriculumDisciplines { get; set; }
    }
}
