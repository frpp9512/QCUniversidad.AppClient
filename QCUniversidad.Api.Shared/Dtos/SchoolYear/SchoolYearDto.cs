using QCUniversidad.Api.Shared.Dtos.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.SchoolYear
{
    public record SchoolYearDto : EditSchoolYearDto
    {
        public IList<CourseDto> Courses { get; set; }
        public int CoursesCount { get; set; }
    }
}
