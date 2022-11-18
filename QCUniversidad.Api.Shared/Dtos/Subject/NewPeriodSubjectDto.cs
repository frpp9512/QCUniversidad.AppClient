using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Shared.Dtos.Subject;

public record NewPeriodSubjectDto
{
    public Guid PeriodId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid CourseId { get; set; }
    public int MidtermExamsCount { get; set; }
    public SubjectTerminationMode TerminationMode { get; set; }
}