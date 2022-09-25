using QCUniversidad.Api.Shared.Dtos.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Curriculum
{
    public record NewCurriculumDto
    {
        public string Denomination { get; set; }
        public string? Description { get; set; }
        public Guid CareerId { get; set; }
        public Guid[] SelectedDisciplines { get; set; }
    }
}