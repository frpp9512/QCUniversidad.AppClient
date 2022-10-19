using QCUniversidad.WebClient.Models.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Planning
{
    public class PlanningIndexModel
    {
        public IList<CourseModel> Courses { get; set; }
        public Guid? CourseSelected { get; set; }
        public Guid? PeriodSelected { get; set; }
    }
}
