using QCUniversidad.Api.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Statistics;

[Display(Name = "Estadística")]
public class StatisticItemModel
{
    public required string Name { get; set; }
    [Display(Name = "Valor de la estadística")]
    public double Value { get; set; }
    public double? RefValue { get; set; }
    public required string Mu { get; set; }
    public StatisticState State { get; set; } = StatisticState.Ok;
    public required string Description { get; set; }
}