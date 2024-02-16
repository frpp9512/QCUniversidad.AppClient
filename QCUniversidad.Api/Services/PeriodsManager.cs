using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Extensions;

namespace QCUniversidad.Api.Services;

public class PeriodsManager(QCUniversidadContext context,
                            ICoefficientCalculator<PeriodModel> periodCalculator) : IPeriodsManager
{
    private readonly QCUniversidadContext _context = context;
    private readonly ICoefficientCalculator<PeriodModel> _periodCalculator = periodCalculator;

    public event EventHandler<Guid> RecalculationRequested = delegate { };

    public async Task<bool> CreatePeriodAsync(PeriodModel period)
    {
        ArgumentNullException.ThrowIfNull(period);

        period.TimeFund = _periodCalculator.CalculateValue(period);
        period.Starts = period.Starts.SetKindUtc();
        period.Ends = period.Ends.SetKindUtc();
        _ = await _context.Periods.AddAsync(period);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> ExistsPeriodAsync(Guid id)
    {
        bool result = await _context.Periods.AnyAsync(d => d.Id == id);
        return result;
    }

    public async Task<IList<PeriodModel>> GetPeriodsAsync(int from, int to)
    {
        List<PeriodModel> result =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.Periods.Skip(from).Take(to).Include(p => p.SchoolYear).ToListAsync()
            : await _context.Periods.Include(p => p.SchoolYear).ToListAsync();
        return result;
    }

    public async Task<int> GetPeriodsCountAsync()
    {
        return await _context.Periods.CountAsync();
    }

    public async Task<PeriodModel> GetPeriodAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            PeriodModel? result = await _context.Periods.Where(p => p.Id == id)
                                               .Include(p => p.SchoolYear)
                                               .FirstOrDefaultAsync();
            return result ?? throw new PeriodNotFoundException();
        }

        throw new ArgumentNullException(nameof(id));
    }

    public async Task<bool> UpdatePeriodAsync(PeriodModel period)
    {
        ArgumentNullException.ThrowIfNull(period);

        period.TimeFund = _periodCalculator.CalculateValue(period);
        bool recalculateTeachers = false;
        if (period.Id != Guid.Empty)
        {
            double currentMonthsCount = await GetPeriodMonthsCountAsync(period.Id);
            double newMonthsCount = period.MonthsCount;
            recalculateTeachers = (currentMonthsCount != newMonthsCount) && await IsPeriodInCurrentYear(period.Id);
        }

        period.Starts = period.Starts.SetKindUtc();
        period.Ends = period.Ends.SetKindUtc();
        _ = _context.Periods.Update(period);
        int result = await _context.SaveChangesAsync();

        if (recalculateTeachers)
        {
            RecalculationRequested?.Invoke(this, period.Id);
        }

        return result > 0;
    }

    public async Task<bool> DeletePeriodAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        try
        {
            PeriodModel period = await GetPeriodAsync(id);
            _ = _context.Periods.Remove(period);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (PeriodNotFoundException)
        {
            throw;
        }
    }

    public async Task<double> GetPeriodTimeFund(Guid periodId)
    {
        if (periodId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(periodId));
        }

        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        PeriodModel period = await _context.Periods.FirstAsync(p => p.Id == periodId);
        return period.TimeFund;
    }

    public async Task<IList<PeriodModel>> GetPeriodsOfSchoolYearAsync(Guid schoolYear)
    {
        IQueryable<PeriodModel> query = from period in _context.Periods
                                        where period.SchoolYearId == schoolYear
                                        select period;

        return await query.ToListAsync();
    }

    public async Task<int> GetSchoolYearPeriodsCountAsync(Guid schoolYearId)
    {
        return await _context.Periods.CountAsync(p => p.SchoolYearId == schoolYearId);
    }

    public async Task<bool> IsPeriodInCurrentYear(Guid periodId)
    {
        IQueryable<bool> query = from period in _context.Periods
                                 join schoolYear in _context.SchoolYears
                                 on period.SchoolYearId equals schoolYear.Id
                                 where period.Id == periodId
                                 select schoolYear.Current;

        return await query.FirstAsync();
    }

    public async Task<double> GetPeriodMonthsCountAsync(Guid periodId)
    {
        IQueryable<double> monthsCountQuery = from period in _context.Periods
                                              where period.Id == periodId
                                              select period.MonthsCount;
        double monthsCount = await monthsCountQuery.FirstAsync();
        return monthsCount;
    }

    public async Task<bool> IsLastPeriodAsync(Guid periodId)
    {
        IQueryable<Guid> schoolYearQuery = from period in _context.Periods
                                           where period.Id == periodId
                                           select period.SchoolYearId;
        if (!await schoolYearQuery.AnyAsync())
        {
            throw new PeriodNotFoundException();
        }

        Guid schoolYearId = await schoolYearQuery.FirstAsync();
        var lastPeriodQuery = await _context.Periods.Where(p => p.SchoolYearId == schoolYearId)
                                               .Select(p => new { p.Id, Starts = p.Starts.DateTime })
                                               .ToListAsync();
        var lastPeriod = lastPeriodQuery.OrderByDescending(p => p.Starts).First();
        return lastPeriod.Id == periodId;
    }
}
