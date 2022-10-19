using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Teacher;

public record TeacherLoadDto
{
    public Guid TeacherId { get; set; }
    public Guid PeriodId { get; set; }
    public double Load { get; set; }
    public double TimeFund { get; set; }
    public double LoadPercent { get; set; }
}
