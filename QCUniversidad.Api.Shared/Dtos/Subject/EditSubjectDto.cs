namespace QCUniversidad.Api.Shared.Dtos.Subject;

public record EditSubjectDto : NewSubjectDto
{
    public Guid Id { get; set; }
}
