using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Services;

public class CareersManager(QCUniversidadContext context) : ICareersManager
{
    private readonly QCUniversidadContext _context = context;

    public async Task<bool> ExistsCareerAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(null, nameof(id));
        }

        bool result = await _context.Careers.AnyAsync(c => c.Id == id);
        return result;
    }

    public async Task<bool> CreateCareerAsync(CareerModel career)
    {
        ArgumentNullException.ThrowIfNull(career);

        _ = await _context.Careers.AddAsync(career);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<IList<CareerModel>> GetCareersAsync(int from = 0, int to = 0)
    {
        List<CareerModel> result = !(from == 0 && to == from)
                     ? await _context.Careers.Skip(from).Take(to).Include(c => c.Faculty).ToListAsync()
                     : await _context.Careers.Include(c => c.Faculty).ToListAsync();
        return result;
    }

    public async Task<IList<CareerModel>> GetCareersAsync(Guid facultyId)
    {
        if (facultyId != Guid.Empty)
        {
            List<CareerModel> result = await _context.Careers.Where(c => c.FacultyId == facultyId).Include(c => c.Faculty).ToListAsync();
            return result;
        }

        throw new ArgumentNullException(nameof(facultyId));
    }

    public async Task<IList<CareerModel>> GetCareersForDepartmentAsync(Guid departmentId)
    {
        if (departmentId != Guid.Empty)
        {
            IQueryable<CareerModel> query = from career in _context.Careers
                                            join departmentCareer in _context.DepartmentsCareers
                                            on career.Id equals departmentCareer.CareerId
                                            where departmentCareer.DepartmentId == departmentId
                                            select career;
            List<CareerModel> result = await query.Include(c => c.Faculty).ToListAsync();
            return result;
        }

        throw new ArgumentNullException(nameof(departmentId));
    }

    public async Task<CareerModel> GetCareerAsync(Guid careerId)
    {
        if (careerId != Guid.Empty)
        {
            CareerModel? result = await _context.Careers.Where(c => c.Id == careerId)
                                               .Include(c => c.Faculty)
                                               .Include(c => c.CareerDepartments)
                                                    .ThenInclude(cd => cd.Department)
                                               .FirstOrDefaultAsync();
            return result ?? throw new CareerNotFoundException();
        }

        throw new ArgumentNullException(nameof(careerId));
    }

    public async Task<int> GetCareersCountAsync()
    {
        int result = await _context.Careers.CountAsync();
        return result;
    }

    public async Task<bool> UpdateCareerAsync(CareerModel career)
    {
        if (career is not null)
        {
            _ = _context.Careers.Update(career);
            int result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                IQueryable<TeachingPlanItemModel> query = from planItem in _context.TeachingPlanItems
                                                          join course in _context.Courses
                                                          on planItem.CourseId equals course.Id
                                                          where course.CareerId == career.Id && planItem.FromPostgraduateCourse != career.PostgraduateCourse
                                                          select planItem;

                if (query.Any())
                {
                    await query.ForEachAsync(i =>
                    {
                        i.FromPostgraduateCourse = career.PostgraduateCourse;
                        _ = _context.Update(i);
                    });
                    _ = await _context.SaveChangesAsync();
                }
            }

            return result > 0;
        }

        throw new ArgumentNullException(nameof(career));
    }

    public async Task<bool> DeleteCareerAsync(Guid careerId)
    {
        if (careerId != Guid.Empty)
        {
            try
            {
                CareerModel career = await GetCareerAsync(careerId);
                _ = _context.Careers.Remove(career);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (CareerNotFoundException)
            {
                throw;
            }
        }

        throw new ArgumentNullException(nameof(careerId));
    }
}
