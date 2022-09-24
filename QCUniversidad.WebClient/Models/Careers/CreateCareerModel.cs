using QCUniversidad.WebClient.Models.Faculties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Careers
{
    public class CreateCareerModel : CareerModel
    {
        public IList<FacultyModel>? Faculties { get; set; }
    }
}
