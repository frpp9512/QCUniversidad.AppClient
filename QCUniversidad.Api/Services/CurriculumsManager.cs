using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;

namespace QCUniversidad.Api.Services;

public class CurriculumsManager(QCUniversidadContext context,
                                ICareersManager careersManager) : ICurriculumsManager
{
    private readonly QCUniversidadContext _context = context;
    private readonly ICareersManager _careersManager = careersManager;

    public async Task<bool> CreateCurriculumAsync(CurriculumModel curriculum)
    {
        if (curriculum is null)
        {
            throw new ArgumentNullException(nameof(curriculum));
        }

        _ = await _context.Curriculums.AddAsync(curriculum);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> ExistsCurriculumAsync(Guid id)
    {
        bool result = await _context.Curriculums.AnyAsync(t => t.Id == id);
        return result;
    }

    public async Task<int> GetCurriculumsCountAsync() => await _context.Curriculums.CountAsync();

    public async Task<int> GetCurriculumDisciplinesCountAsync(Guid id)
    {
        int result = await _context.CurriculumsDisciplines.CountAsync(td => td.CurriculumId == id);
        return result;
    }

    public async Task<IList<CurriculumModel>> GetCurriculumsAsync(int from, int to)
    {
        List<CurriculumModel> result =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.Curriculums.Skip(from).Take(to).Include(c => c.Career).Include(c => c.CurriculumDisciplines).ThenInclude(cs => cs.Discipline).ToListAsync()
            : await _context.Curriculums.Include(c => c.Career).Include(c => c.CurriculumDisciplines).ThenInclude(cs => cs.Discipline).ToListAsync();
        return result;
    }

    public async Task<IList<CurriculumModel>> GetCurriculumsForCareerAsync(Guid careerId)
    {
        if (careerId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(careerId));
        }

        try
        {
            if (!await _careersManager.ExistsCareerAsync(careerId))
            {
                throw new CareerNotFoundException();
            }

            List<CurriculumModel> curriculums = await _context.Curriculums.Where(c => c.CareerId == careerId).ToListAsync();
            return curriculums;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<CurriculumModel> GetCurriculumAsync(Guid id)
    {
        CurriculumModel? result = await _context.Curriculums.Where(t => t.Id == id)
                                                   .Include(c => c.Career)
                                                   .Include(c => c.CurriculumDisciplines)
                                                   .ThenInclude(cs => cs.Discipline)
                                                   .FirstOrDefaultAsync();
        return result ?? throw new TeacherNotFoundException();
    }

    public async Task<bool> UpdateCurriculumAsync(CurriculumModel curriculum)
    {
        ArgumentNullException.ThrowIfNull(curriculum);

        await _context.CurriculumsDisciplines.Where(td => td.CurriculumId == curriculum.Id)
                                              .ForEachAsync(td => _context.Remove(td));
        await _context.CurriculumsDisciplines.AddRangeAsync(curriculum.CurriculumDisciplines);
        _ = _context.Curriculums.Update(curriculum);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteCurriculumAsync(Guid id)
    {
        try
        {
            CurriculumModel curriculum = await GetCurriculumAsync(id);
            _ = _context.Curriculums.Remove(curriculum);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (CurriculumNotFoundException)
        {
            throw;
        }
    }
}
