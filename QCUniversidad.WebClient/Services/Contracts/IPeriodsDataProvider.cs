using QCUniversidad.WebClient.Models.Periods;

namespace QCUniversidad.WebClient.Services.Contracts;

public interface IPeriodsDataProvider
{
    Task<bool> CreatePeriodAsync(PeriodModel period);
    Task<bool> ExistsPeriodAsync(Guid id);
    Task<int> GetPeriodsCountAsync();
    Task<IList<PeriodModel>> GetPeriodsAsync(int from, int to);
    Task<IList<PeriodModel>> GetPeriodsAsync(Guid? schoolYearId = null);
    Task<PeriodModel> GetPeriodAsync(Guid id);
    Task<bool> UpdatePeriodAsync(PeriodModel period);
    Task<bool> DeletePeriodAsync(Guid id);
}
