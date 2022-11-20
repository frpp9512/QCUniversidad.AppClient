namespace QCUniversidad.Api.Shared.Dtos.Teacher;

public record EditTeacherDto : NewTeacherDto
{
    public Guid Id { get; set; }
}
