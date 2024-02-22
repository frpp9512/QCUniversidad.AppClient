using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Dtos.Period;

namespace QCUniversidad.Api.Shared.Dtos.SchoolYear;

public record SchoolYearDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool Current { get; set; }
    public IList<CourseDto>? Courses { get; set; }
    public int CoursesCount { get; set; }
    public IList<SimplePeriodDto>? Periods { get; set; }
    public int PeriodsCount { get; set; }
}
