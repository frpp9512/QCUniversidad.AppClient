namespace QCUniversidad.Api.Shared.Dtos.Subject;
public record EditPeriodSubjectDto : NewPeriodSubjectDto
{
    public Guid Id { get; set; }
}
