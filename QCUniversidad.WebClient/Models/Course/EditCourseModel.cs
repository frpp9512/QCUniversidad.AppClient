using QCUniversidad.WebClient.Models.Curriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Courses
{
    public record EditCourseModel : CourseModel
    {
        public IList<CurriculumModel>? Curricula { get; set; }
    }
}
