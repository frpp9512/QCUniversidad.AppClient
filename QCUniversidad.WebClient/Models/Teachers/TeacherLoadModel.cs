using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Teachers;

public class TeacherLoadModel
{
    public Guid TeacherId { get; set; }
    public Guid PeriodId { get; set; }
    public double Load { get; set; }
    public double TimeFund { get; set; }
    public double LoadPercent { get; set; }
}