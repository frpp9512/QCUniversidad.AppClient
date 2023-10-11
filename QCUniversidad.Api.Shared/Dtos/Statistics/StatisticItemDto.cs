﻿using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Shared.Dtos.Statistics;

public class StatisticItemDto
{
    public required string Name { get; set; }
    public double Value { get; set; }
    public double? RefValue { get; set; }
    public string? Mu { get; set; }
    public StatisticState State { get; set; } = StatisticState.Ok;
    public string? Description { get; set; }
}