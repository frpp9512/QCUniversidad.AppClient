using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Disciplines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Curriculums
{
    public class CreateCurriculumModel : CurriculumModel
    {
        public IList<CareerModel>? Careers { get; set; }
        public IList<DisciplineModel>? Disciplines { get; set; }
    }
}