namespace QCUniversidad.Api.Shared.Dtos.Subject;

public record SimplePeriodSubjectDto : EditPeriodSubjectDto
{
    public required SubjectDto Subject { get; set; }
}
