using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.WebClient.Models.Subjects;

public class CreatePeriodSubjectModel
{
    public Guid PeriodId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid CourseId { get; set; }
    public int MidtermExamsCount { get; set; }
    public SubjectTerminationMode TerminationMode { get; set; }
    public double HoursPlanned { get; set; }
}