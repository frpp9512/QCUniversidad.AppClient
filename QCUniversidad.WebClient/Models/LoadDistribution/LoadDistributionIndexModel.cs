using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCUniversidad.WebClient.Models.SchoolYears;

namespace QCUniversidad.WebClient.Models.LoadDistribution
{
    public class LoadDistributionIndexModel
    {
        public SchoolYearModel SchoolYear { get; set; }
        public DepartmentModel Department { get; set; }
        public IList<CourseModel> Courses { get; set; }
    }
}
