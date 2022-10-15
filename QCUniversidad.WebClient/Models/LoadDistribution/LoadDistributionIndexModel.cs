using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.SchoolYears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.LoadDistribution
{
    public class LoadDistributionIndexModel
    {
        public DepartmentModel Department { get; set; }
        public IList<SchoolYearModel> SchoolYears { get; set; }
    }
}
