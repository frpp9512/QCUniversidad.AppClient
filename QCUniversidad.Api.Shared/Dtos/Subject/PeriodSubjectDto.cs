using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Dtos.Period;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Subject;

public record PeriodSubjectDto : SimplePeriodSubjectDto
{
    public PeriodDto Period { get; set; }
    public SimpleCourseDto Course { get; set; }
}
