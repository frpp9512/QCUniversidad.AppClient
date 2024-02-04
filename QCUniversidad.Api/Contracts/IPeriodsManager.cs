using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Contracts;

public interface IPeriodsManager
{
    event EventHandler<Guid> RecalculationRequested;

    Task<bool> CreatePeriodAsync(PeriodModel period);
    Task<bool> ExistsPeriodAsync(Guid id);
    Task<int> GetPeriodsCountAsync();
    Task<int> GetSchoolYearPeriodsCountAsync(Guid schoolYear);
    Task<IList<PeriodModel>> GetPeriodsAsync(int from, int to);
    Task<PeriodModel> GetPeriodAsync(Guid id);
    Task<bool> UpdatePeriodAsync(PeriodModel period);
    Task<bool> DeletePeriodAsync(Guid id);
    Task<IList<PeriodModel>> GetPeriodsOfSchoolYearAsync(Guid schoolYear);
    Task<double> GetPeriodTimeFund(Guid periodId);
    Task<bool> IsPeriodInCurrentYear(Guid periodId);
    Task<double> GetPeriodMonthsCountAsync(Guid periodId);
    Task<bool> IsLastPeriodAsync(Guid periodId);
}
