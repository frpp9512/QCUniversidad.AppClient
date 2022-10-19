using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Curriculums;
using QCUniversidad.WebClient.Models.SchoolYears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Courses
{
    public record CreateCourseModel : CourseModel
    {
        public IList<SchoolYearModel>? SchoolYears { get; set; }
        public IList<CareerModel>? Careers { get; set; }
        public IList<CurriculumModel>? Curricula { get; set; }
    }
}