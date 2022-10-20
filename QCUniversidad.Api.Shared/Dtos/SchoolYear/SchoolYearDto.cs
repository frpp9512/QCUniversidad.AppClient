using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Dtos.Period;
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
        public IList<SimplePeriodDto> Periods { get; set; }
        public int PeriodsCount { get; set; }
    }
}
