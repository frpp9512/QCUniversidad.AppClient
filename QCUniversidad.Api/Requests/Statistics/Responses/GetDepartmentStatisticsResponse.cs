using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Statistics;

namespace QCUniversidad.Api.Requests.Statistics.Responses;

public record GetDepartmentStatisticsResponse : ResponseBase
{
    public List<StatisticItemDto>? StatisticItems { get; set; }

    public override object? GetPayload() => StatisticItems;
}
