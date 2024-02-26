using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;

namespace QCUniversidad.Api.Services;

public class FacultiesManager(QCUniversidadContext context) : IFacultiesManager
{
    private readonly QCUniversidadContext _context = context;

    public async Task<FacultyModel> GetFacultyAsync(Guid id)
    {
        FacultyModel? faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Id.Equals(id));
        return faculty is null ? throw new FacultyNotFoundException() : faculty;
    }

    public async Task<IList<FacultyModel>> GetFacultiesAsync(int from = 0, int to = 0)
    {
        List<FacultyModel> faculties =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.Faculties.OrderBy(f => f.Name).Skip(from).Take(to).ToListAsync()
            : await _context.Faculties.OrderBy(f => f.Name).ToListAsync();
        return faculties;
    }

    public async Task<int> GetFacultiesTotalAsync()
    {
        int total = await _context.Faculties.CountAsync();
        return total;
    }

    public async Task<bool> ExistFacultyAsync(Guid id)
    {
        bool result = await _context.Faculties.AnyAsync(f => f.Id == id);
        return result;
    }

    public async Task<FacultyModel?> CreateFacultyAsync(FacultyModel faculty)
    {
        ArgumentNullException.ThrowIfNull(faculty);

        _ = await _context.AddAsync(faculty);
        int result = await _context.SaveChangesAsync();
        return result > 0 ? faculty : null;
    }

    public async Task<bool> UpdateFacultyAsync(FacultyModel faculty)
    {
        ArgumentNullException.ThrowIfNull(faculty);

        _ = _context.Update(faculty);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<int> GetFacultyDepartmentCountAsync(Guid facultyId)
    {
        int count = await _context.Departments.CountAsync(d => d.FacultyId == facultyId);
        return count;
    }

    public async Task<int> GetFacultyCareerCountAsync(Guid facultyId)
    {
        int count = await _context.Careers.CountAsync(c => c.FacultyId.Equals(facultyId));
        return count;
    }

    public async Task<bool> DeleteFacultyAsync(Guid facultyId)
    {
        try
        {
            FacultyModel faculty = await GetFacultyAsync(facultyId);
            _ = _context.Remove(faculty);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (FacultyNotFoundException)
        {
            throw;
        }
    }
}
