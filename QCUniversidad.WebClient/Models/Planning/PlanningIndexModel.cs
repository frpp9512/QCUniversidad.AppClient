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
        public IList<SchoolYearModel> SchoolYears { get; set; }
        public Guid? SchoolYearSelected { get; set; }
        public Guid? PeriodSelected { get; set; }
    }
}
