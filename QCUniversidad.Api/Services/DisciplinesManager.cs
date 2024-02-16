using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Services;

public class DisciplinesManager(QCUniversidadContext context,
                                IDepartmentsManager departmentsManager,
                                ITeachersManager teachersManager) : IDisciplinesManager
{
    private readonly QCUniversidadContext _context = context;
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;
    private readonly ITeachersManager _teachersManager = teachersManager;

    public async Task<bool> CreateDisciplineAsync(DisciplineModel discipline)
    {
        ArgumentNullException.ThrowIfNull(discipline);

        _ = await _context.Disciplines.AddAsync(discipline);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> ExistsDisciplineAsync(Guid id)
    {
        bool result = await _context.Disciplines.AnyAsync(d => d.Id == id);
        return result;
    }

    public async Task<bool> ExistsDisciplineAsync(string name)
    {
        bool result = await _context.Disciplines.AnyAsync(d => d.Name == name);
        return result;
    }

    public async Task<IList<DisciplineModel>> GetDisciplinesAsync(int from, int to)
    {
        List<DisciplineModel> result =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.Disciplines.Skip(from).Take(to).Include(d => d.Department).ToListAsync()
            : await _context.Disciplines.Include(d => d.Department).ToListAsync();
        return result;
    }

    public async Task<IList<DisciplineModel>> GetDisciplinesAsync(Guid departmentId)
    {
        if (!await _departmentsManager.ExistDepartmentAsync(departmentId))
        {
            throw new DepartmentNotFoundException();
        }

        List<DisciplineModel> result = await _context.Disciplines.Where(d => d.DepartmentId == departmentId).ToListAsync();
        return result;
    }

    public async Task<int> GetDisciplinesCountAsync()
    {
        return await _context.Disciplines.CountAsync();
    }

    public async Task<int> GetDisciplineSubjectsCountAsync(Guid disciplineId)
    {
        int result = await _context.Subjects.CountAsync(s => s.DisciplineId == disciplineId);
        return result;
    }

    public async Task<int> GetDisciplineTeachersCountAsync(Guid disciplineId)
    {
        int result = await _context.TeachersDisciplines.CountAsync(td => td.DisciplineId == disciplineId);
        return result;
    }

    public async Task<DisciplineModel> GetDisciplineAsync(Guid disciplineId)
    {
        DisciplineModel? result = await _context.Disciplines.Where(d => d.Id == disciplineId)
                                                   .Include(d => d.Department)
                                                   .FirstOrDefaultAsync();
        return result ?? throw new DisciplineNotFoundException();
    }

    public async Task<DisciplineModel> GetDisciplineAsync(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        DisciplineModel? result = await _context.Disciplines.Where(d => d.Name == name)
                                                   .Include(d => d.Department)
                                                   .FirstOrDefaultAsync();
        return result ?? throw new DisciplineNotFoundException();
    }

    public async Task<bool> UpdateDisciplineAsync(DisciplineModel discipline)
    {
        if (discipline is null)
        {
            throw new ArgumentNullException(nameof(discipline));
        }

        _ = _context.Disciplines.Update(discipline);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteDisciplineAsync(Guid disciplineId)
    {
        try
        {
            DisciplineModel discipline = await GetDisciplineAsync(disciplineId);
            _ = _context.Disciplines.Remove(discipline);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (DisciplineNotFoundException)
        {
            throw;
        }
    }

    public async Task<IList<DisciplineModel>> GetDisciplinesForTeacher(Guid teacherId)
    {
        if (!await _teachersManager.ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }

        IQueryable<DisciplineModel> disciplines = from td in _context.TeachersDisciplines
                                                  join discipline in _context.Disciplines
                                                  on td.DisciplineId equals discipline.Id
                                                  where td.TeacherId == teacherId
                                                  select discipline;
        disciplines = disciplines.Include(d => d.Department);
        return await disciplines.ToListAsync();
    }
}
