using Microsoft.AspNetCore.Cors;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    [Display(Name = "Carga")]
    public double LoadPercent { get; set; }

    public TeacherLoadStatus Status { get; set; }
}