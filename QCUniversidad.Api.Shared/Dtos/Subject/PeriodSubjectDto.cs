using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Dtos.Period;

namespace QCUniversidad.Api.Shared.Dtos.Subject;

public record PeriodSubjectDto : SimplePeriodSubjectDto
{
    public PeriodDto? Period { get; set; }
    public SimpleCourseDto? Course { get; set; }
}
