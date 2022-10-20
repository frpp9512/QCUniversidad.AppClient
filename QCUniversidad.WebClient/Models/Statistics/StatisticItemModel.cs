using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Statistics;

public class StatisticItemModel
{
    public string Name { get; set; }
    public double Value { get; set; }
    public double? RefValue { get; set; }
    public string Mu { get; set; }
    public StatisticState State { get; set; } = StatisticState.Ok;
    public string Description { get; set; }
}