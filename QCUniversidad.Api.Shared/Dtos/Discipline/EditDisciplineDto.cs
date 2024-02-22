namespace QCUniversidad.Api.Shared.Dtos.Discipline;

public record EditDisciplineDto : NewDisciplineDto
{
    public Guid Id { get; set; }
}
