using QCUniversidad.WebClient.Models.Statistics;

namespace QCUniversidad.WebClient.Services.Contracts;

public interface IStatisticsDataProvider
{
    Task<IList<StatisticItemModel>> GetGlobalStatisticsAsync();
    Task<IList<StatisticItemModel>> GetGlobalStatisticsForDepartmentAsync(Guid departmentId);
}
