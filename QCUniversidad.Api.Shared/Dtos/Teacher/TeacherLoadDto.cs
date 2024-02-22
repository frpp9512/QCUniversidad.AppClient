using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Shared.Dtos.Teacher;

public record TeacherLoadDto
{
    public Guid TeacherId { get; set; }
    public Guid PeriodId { get; set; }
    public double Load { get; set; }
    public double TimeFund { get; set; }
    public double LoadPercent { get; set; }
    public TeacherLoadStatus Status { get; set; }
}