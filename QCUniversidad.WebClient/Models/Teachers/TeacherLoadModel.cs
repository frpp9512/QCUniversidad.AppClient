using QCUniversidad.Api.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Teachers;

public class TeacherLoadModel
{
    public Guid TeacherId { get; set; }
    public Guid PeriodId { get; set; }
    public double Load { get; set; }
    public double TimeFund { get; set; }

    [Display(Name = "Carga")]
    public double LoadPercent { get; set; }

    public TeacherLoadStatus Status { get; set; }
}