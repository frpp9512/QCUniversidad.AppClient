using QCUniversidad.WebClient.Models.Curriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.SchoolYears
{
    public record EditSchoolYearModel : SchoolYearModel
    {
        public IList<CurriculumModel>? Curricula { get; set; }
    }
}
