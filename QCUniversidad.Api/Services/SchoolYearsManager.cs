using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Services;

public class SchoolYearsManager(QCUniversidadContext context) : ISchoolYearsManager
{
    private readonly QCUniversidadContext _context = context;

    public async Task<SchoolYearModel> GetCurrentSchoolYearAsync()
    {
        try
        {
            return await _context.SchoolYears.Include(sy => sy.Periods).Where(sy => sy.Current).FirstAsync() ?? throw new NotCurrentSchoolYearDefined();
        }
        catch (NotCurrentSchoolYearDefined)
        {
            throw;
        }
    }

    public async Task<SchoolYearModel> GetSchoolYearAsync(Guid id)
    {
        SchoolYearModel? schoolYear = await _context.SchoolYears.Include(sy => sy.Periods).FirstOrDefaultAsync(sy => sy.Id.Equals(id));
        return schoolYear is null ? throw new SchoolYearNotFoundException() : schoolYear;
    }

    public async Task<IList<SchoolYearModel>> GetSchoolYearsAsync(int from = 0, int to = 0)
    {
        List<SchoolYearModel> schoolYears =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.SchoolYears.Include(sy => sy.Periods).Skip(from).Take(to).ToListAsync()
            : await _context.SchoolYears.Include(sy => sy.Periods).ToListAsync();
        return schoolYears.OrderByDescending(sy => sy.Current).ThenByDescending(sy => sy.Name).ToList();
    }

    public async Task<int> GetSchoolYearTotalAsync()
    {
        int total = await _context.SchoolYears.CountAsync();
        return total;
    }

    public async Task<bool> ExistSchoolYearAsync(Guid id)
    {
        bool result = await _context.SchoolYears.AnyAsync(f => f.Id == id);
        return result;
    }

    public async Task<bool> CreateSchoolYearAsync(SchoolYearModel schoolYear)
    {
        ArgumentNullException.ThrowIfNull(schoolYear);

        if (schoolYear.Current)
        {
            await RemoveCurrentSchoolYearMark();
        }

        _ = await _context.AddAsync(schoolYear);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateSchoolYearAsync(SchoolYearModel schoolYear)
    {
        ArgumentNullException.ThrowIfNull(schoolYear);

        if (schoolYear.Current)
        {
            await RemoveCurrentSchoolYearMark();
        }

        _ = _context.Update(schoolYear);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<int> GetSchoolYearCoursesCountAsync(Guid schoolYearId)
    {
        int count = await _context.Courses.CountAsync(c => c.SchoolYearId == schoolYearId);
        return count;
    }

    public async Task<bool> DeleteSchoolYearAsync(Guid schoolYearId)
    {
        try
        {
            int goal = 1;
            SchoolYearModel schoolYear = await GetSchoolYearAsync(schoolYearId);
            if (schoolYear.Current && await _context.SchoolYears.CountAsync() > 1)
            {
                SchoolYearModel? nextCurrent = await _context.SchoolYears.OrderByDescending(sy => sy.Name)
                                                            .FirstOrDefaultAsync(sy => sy.Id != schoolYear.Id);
                if (nextCurrent is not null)
                {
                    nextCurrent.Current = true;
                    _ = _context.SchoolYears.Update(nextCurrent);
                    goal++;
                }
            }

            _ = _context.Remove(schoolYear);
            int result = await _context.SaveChangesAsync();
            return result >= goal;
        }
        catch (SchoolYearNotFoundException)
        {
            throw;
        }
    }

    private async Task RemoveCurrentSchoolYearMark()
    {
        IQueryable<SchoolYearModel> query = from schoolYear in _context.SchoolYears
                                            where schoolYear.Current
                                            select schoolYear;
        foreach (SchoolYearModel? schoolYear in query)
        {
            schoolYear.Current = false;
            _ = _context.Update(schoolYear);
        }

        _ = await _context.SaveChangesAsync();
    }
}
