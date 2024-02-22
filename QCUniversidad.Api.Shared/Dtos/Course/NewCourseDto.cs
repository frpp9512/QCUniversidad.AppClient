using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Shared.Dtos.Course;

public record NewCourseDto
{
    public Guid SchoolYearId { get; set; }
    public int CareerYear { get; set; }
    public bool LastCourse { get; set; }
    public required string Denomination { get; set; }
    public TeachingModality TeachingModality { get; set; }
    public uint Enrolment { get; set; }
    public Guid CareerId { get; set; }
    public Guid CurriculumId { get; set; }
}