using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.SchoolYears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.LoadDistribution
{
    public class SelectDepartmentViewModel
    {
        public IList<SchoolYearModel> SchoolYears { get; set; }
        public IList<DepartmentModel> Departments { get; set; }
        public string RedirectTo { get; set; }
    }
}
