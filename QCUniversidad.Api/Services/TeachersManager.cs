using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Services;

public class TeachersManager(QCUniversidadContext context,
                             ISchoolYearsManager schoolYearsManager,
                             IDepartmentsManager departmentsManager) : ITeachersManager
{
    private readonly QCUniversidadContext _context = context;
    private readonly ISchoolYearsManager _schoolYearsManager = schoolYearsManager;
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;

    public event EventHandler<(Guid departmentId, Guid periodId)> RequestLoadRecalculation = delegate { };

    public async Task<bool> CreateTeacherAsync(TeacherModel teacher)
    {
        ArgumentNullException.ThrowIfNull(teacher);

        _ = await _context.Teachers.AddAsync(teacher);
        int result = await _context.SaveChangesAsync();
        if (result <= 0)
        {
            return false;
        }

        SchoolYearModel schoolYear = await _schoolYearsManager.GetCurrentSchoolYearAsync();
        List<PeriodModel> periods = await _context.Periods.Where(p => p.SchoolYearId == schoolYear.Id).ToListAsync();
        foreach (PeriodModel? period in periods)
        {
            RequestLoadRecalculation.Invoke(this, (teacher.DepartmentId, period.Id));
        }

        return true;
    }

    public async Task<bool> ExistsTeacherAsync(Guid id)
    {
        bool result = await _context.Teachers.AnyAsync(t => t.Id == id);
        return result;
    }

    public async Task<bool> ExistsTeacherAsync(string personalId)
    {
        bool result = await _context.Teachers.AnyAsync(t => t.PersonalId == personalId);
        return result;
    }

    public async Task<int> GetTeachersCountAsync()
    {
        return await _context.Teachers.CountAsync();
    }

    public async Task<int> GetTeacherDisciplinesCountAsync(Guid id)
    {
        int result = await _context.TeachersDisciplines.CountAsync(td => td.TeacherId == id);
        return result;
    }

    public async Task<IList<TeacherModel>> GetTeachersAsync(int from = 0, int to = 0)
    {
        List<TeacherModel> result =
            !(from == 0 && to == from)
            ? await _context.Teachers.Where(t => t.Active).Skip(from).Take(to).Include(d => d.Department).Include(d => d.TeacherDisciplines).ThenInclude(td => td.Discipline).ToListAsync()
            : await _context.Teachers.Where(t => t.Active).Include(d => d.Department).Include(d => d.TeacherDisciplines).ThenInclude(td => td.Discipline).ToListAsync();
        return result;
    }

    public async Task<TeacherModel> GetTeacherAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        TeacherModel? result = await _context.Teachers.Where(t => t.Id == id)
                                            .Include(d => d.Department)
                                            .Include(d => d.TeacherDisciplines).ThenInclude(td => td.Discipline)
                                            .FirstOrDefaultAsync();
        return result ?? throw new TeacherNotFoundException();
    }

    public async Task<TeacherModel> GetTeacherAsync(string personalId)
    {
        if (string.IsNullOrEmpty(personalId))
        {
            throw new ArgumentNullException(nameof(personalId));
        }

        TeacherModel? result = await _context.Teachers.Where(t => t.PersonalId == personalId)
                                                .Include(d => d.Department)
                                                .Include(d => d.TeacherDisciplines).ThenInclude(td => td.Discipline)
                                                .FirstOrDefaultAsync();
        return result ?? throw new TeacherNotFoundException();
    }

    public async Task<bool> UpdateTeacherAsync(TeacherModel teacher)
    {
        if (teacher is null)
        {
            throw new ArgumentNullException(nameof(teacher));
        }

        await _context.TeachersDisciplines.Where(td => td.TeacherId == teacher.Id)
                                          .ForEachAsync(td => _context.Remove(td));
        if (teacher.TeacherDisciplines is not null)
        {
            await _context.TeachersDisciplines.AddRangeAsync(teacher.TeacherDisciplines);
        }

        _ = _context.Teachers.Update(teacher);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteTeacherAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        try
        {
            TeacherModel teacher = await GetTeacherAsync(id);
            if (!await DoTeacherHaveLoad(id))
            {
                _ = _context.Teachers.Remove(teacher);
            }
            else
            {
                teacher.Active = false;
                _ = _context.Teachers.Update(teacher);
            }

            int result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return false;
            }

            SchoolYearModel schoolYear = await _schoolYearsManager.GetCurrentSchoolYearAsync();
            List<PeriodModel> periods = await _context.Periods.Where(p => p.SchoolYearId == schoolYear.Id).ToListAsync();
            foreach (PeriodModel? period in periods)
            {
                RequestLoadRecalculation.Invoke(this, (teacher.DepartmentId, period.Id));
            }

            return true;
        }
        catch (TeacherNotFoundException)
        {
            throw;
        }
    }

    private async Task<bool> DoTeacherHaveLoad(Guid teacherId) => !await ExistsTeacherAsync(teacherId)
                                                                    ? throw new TeacherNotFoundException()
                                                                    : await _context.LoadItems.AnyAsync(l => l.TeacherId == teacherId);

    public async Task<IList<TeacherModel>> GetTeachersOfDepartmentAsync(Guid departmentId, bool loadInactives = false)
    {
        try
        {
            if (!await _departmentsManager.ExistDepartmentAsync(departmentId))
            {
                throw new ArgumentException("The department is invalid.", nameof(departmentId));
            }

            IQueryable<TeacherModel> query = from t in _context.Teachers
                                             where (loadInactives || t.Active) && t.DepartmentId == departmentId
                                             select t;
            return await query.Include(t => t.TeacherDisciplines).ThenInclude(td => td.Discipline).ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IList<TeacherModel>> GetTeachersOfFacultyAsync(Guid facultyId, bool loadInactives = false)
    {
        try
        {
            if (!await _departmentsManager.ExistDepartmentAsync(facultyId))
            {
                throw new ArgumentNullException();
            }

            IQueryable<TeacherModel> query = from t in _context.Teachers
                                             join d in _context.Departments
                                             on t.DepartmentId equals d.Id
                                             where (loadInactives || t.Active) && d.FacultyId == facultyId
                                             select t;
            return await query.Include(t => t.TeacherDisciplines).ThenInclude(td => td.Discipline).ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IList<TeacherModel>> GetSupportTeachersAsync(Guid departmentId, Guid periodId)
    {
        if (departmentId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(departmentId));
        }

        if (!await _departmentsManager.ExistDepartmentAsync(departmentId))
        {
            throw new DepartmentNotFoundException();
        }

        IQueryable<DisciplineModel> disciplines = from discipline in _context.Disciplines
                                                  where discipline.DepartmentId == departmentId
                                                  select discipline;

        IQueryable<SubjectModel> subjects = from subject in _context.Subjects
                                            join discipline in disciplines
                                            on subject.DisciplineId equals discipline.Id
                                            select subject;

        IQueryable<TeachingPlanItemModel> planItems = from planItem in _context.TeachingPlanItems
                                                      join subject in subjects
                                                      on planItem.SubjectId equals subject.Id
                                                      where planItem.PeriodId == periodId
                                                      select planItem;

        IQueryable<LoadItemModel> loadItems = from loadItem in _context.LoadItems
                                              join planItem in planItems
                                              on loadItem.PlanningItemId equals planItem.Id
                                              select loadItem;

        IQueryable<TeacherModel> teachers = from teacher in _context.Teachers
                                            join loadItem in loadItems
                                            on teacher.Id equals loadItem.TeacherId
                                            where teacher.DepartmentId != departmentId
                                            select teacher;

        return await teachers.Include(t => t.TeacherDisciplines).ThenInclude(td => td.Discipline).Include(t => t.Department).ToListAsync();
    }
}
