using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Statistics;

public class StatisticItemDto
{
    public string Name { get; set; }
    public double Value { get; set; }
    public double? RefValue { get; set; }
    public string Mu { get; set; }
    public StatisticState State { get; set; }
    public string Description { get; set; }
}