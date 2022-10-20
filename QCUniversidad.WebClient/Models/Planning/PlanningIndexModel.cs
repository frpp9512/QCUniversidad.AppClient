using QCUniversidad.WebClient.Models.Courses;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.SchoolYears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Planning
{
    public class PlanningIndexModel
    {
        public Guid SchoolYearId { get; set; }
        public SchoolYearModel SchoolYear { get; set; }
        public IList<PeriodModel> Periods { get; set; }
        public Guid? PeriodSelected { get; set; }
    }
}