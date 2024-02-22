namespace QCUniversidad.Api.Shared.Dtos.Curriculum;

public record EditCurriculumDto : NewCurriculumDto
{
    public Guid Id { get; set; }
}
