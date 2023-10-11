namespace QCUniversidad.Api.Shared.Dtos.Course;

public record EditCourseDto : NewCourseDto
{
    public Guid Id { get; set; }
}
