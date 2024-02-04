using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Services;

public class DepartmentsManager(QCUniversidadContext context,
                                IPeriodsManager periodsManager,
                                IFacultiesManager facultiesManager) : IDepartmentsManager
{
    private readonly QCUniversidadContext _context = context;
    private readonly IPeriodsManager _periodsManager = periodsManager;
    private readonly IFacultiesManager _facultiesManager = facultiesManager;

    public async Task<IList<DepartmentModel>> GetDepartmentsAsync(int from, int to)
    {
        List<DepartmentModel> deparments = !(from == 0 && to == from)
                         ? await _context.Departments.OrderBy(d => d.Name).Skip(from).Take(to).Include(d => d.Faculty).ToListAsync()
                         : await _context.Departments.OrderBy(d => d.Name).Include(d => d.Faculty).ToListAsync();
        return deparments;
    }

    public async Task<int> GetDepartmentDisciplinesCount(Guid departmentId)
    {
        int count = await _context.Disciplines.CountAsync(d => d.DepartmentId == departmentId);
        return count;
    }

    public async Task<bool> ExistDepartmentAsync(Guid id)
    {
        bool result = await _context.Departments.AnyAsync(f => f.Id == id);
        return result;
    }

    public async Task<IList<DepartmentModel>> GetDepartmentsAsync(Guid facultyId)
    {
        try
        {
            if (await _facultiesManager.ExistFacultyAsync(facultyId))
            {
                List<DepartmentModel> deparments = await _context.Departments.Where(d => d.FacultyId == facultyId).OrderBy(d => d.Name).ToListAsync();
                return deparments;
            }

            throw new FacultyNotFoundException();
        }
        catch
        {
            throw;
        }
    }

    public async Task<int> GetDepartmentsCountAsync()
    {
        int count = await _context.Departments.CountAsync();
        return count;
    }

    public async Task<int> GetDepartmentsCountAsync(Guid facultyId)
    {
        int count = await _context.Departments.CountAsync(d => d.FacultyId == facultyId);
        return count;
    }

    public async Task<DepartmentModel> GetDepartmentAsync(Guid departmentId)
    {
        DepartmentModel? department = await _context.Departments.Where(d => d.Id == departmentId)
                                                   .Include(d => d.Faculty)
                                                   .Include(d => d.DepartmentCareers)
                                                        .ThenInclude(dc => dc.Career)
                                                   .FirstOrDefaultAsync();
        return department ?? throw new DepartmentNotFoundException();
    }

    public async Task<int> GetDeparmentTeachersCountAsync(Guid departmentId)
    {
        if (!await ExistDepartmentAsync(departmentId))
        {
            throw new DepartmentNotFoundException();
        }

        int count = await _context.Teachers.CountAsync(t => t.DepartmentId == departmentId);
        return count;
    }

    public async Task<bool> CreateDepartmentAsync(DepartmentModel department)
    {
        if (department is null)
        {
            throw new ArgumentNullException(nameof(department));
        }

        _ = await _context.Departments.AddAsync(department);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateDeparmentAsync(DepartmentModel department)
    {
        if (department is null)
        {
            throw new ArgumentNullException(nameof(department));
        }

        await _context.DepartmentsCareers.Where(dc => dc.DepartmentId == department.Id)
                                         .ForEachAsync(dc => _context.Remove(dc));
        _ = _context.Departments.Update(department);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteDeparmentAsync(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(departmentId));
        }

        DepartmentModel department = await GetDepartmentAsync(departmentId);
        _ = _context.Departments.Remove(department);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<double> GetTotalLoadInPeriodAsync(Guid periodId)
    {
        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        IQueryable<double> query = from planItem in _context.TeachingPlanItems
                                   where planItem.PeriodId == periodId
                                   select planItem.TotalHoursPlanned;

        double result = await query.SumAsync();

        return result;
    }

    public async Task<double> GetDepartmentTotalLoadInPeriodAsync(Guid periodId, Guid departmentId)
    {
        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        IQueryable<double> directLoadQuery = from loadItem in _context.LoadItems
                                             join teacher in _context.Teachers
                                             on loadItem.TeacherId equals teacher.Id
                                             join planItem in _context.TeachingPlanItems
                                             on loadItem.PlanningItemId equals planItem.Id
                                             where teacher.DepartmentId == departmentId && planItem.PeriodId == periodId
                                             select loadItem.HoursCovered;

        double directLoad = await directLoadQuery.SumAsync();

        IQueryable<double> indirectLoadQuery = from ntLoadItem in _context.NonTeachingLoad
                                               join teacher in _context.Teachers
                                               on ntLoadItem.TeacherId equals teacher.Id
                                               where ntLoadItem.PeriodId == periodId && teacher.DepartmentId == departmentId
                                               select ntLoadItem.Load;

        double indirectLoad = await indirectLoadQuery.SumAsync();

        return directLoad + indirectLoad;
    }

    public async Task<double> GetTotalLoadCoveredInPeriodAsync(Guid periodId)
    {
        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        IQueryable<double> query = from loadItem in _context.LoadItems
                                   join planItem in _context.TeachingPlanItems
                                   on loadItem.PlanningItemId equals planItem.Id
                                   where planItem.PeriodId == periodId
                                   join subject in _context.Subjects
                                   on planItem.SubjectId equals subject.Id
                                   join discipline in _context.Disciplines
                                   on subject.DisciplineId equals discipline.Id
                                   join department in _context.Departments
                                   on discipline.DepartmentId equals department.Id
                                   select loadItem.HoursCovered;

        double result = await query.SumAsync();

        return result;
    }

    public async Task<double> GetDepartmentTotalLoadCoveredInPeriodAsync(Guid periodId, Guid departmentId)
    {
        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        IQueryable<double> loadItemsQuery = from loadItem in _context.LoadItems
                                            join planItem in _context.TeachingPlanItems
                                            on loadItem.PlanningItemId equals planItem.Id
                                            where planItem.PeriodId == periodId
                                            join subject in _context.Subjects
                                            on planItem.SubjectId equals subject.Id
                                            join discipline in _context.Disciplines
                                            on subject.DisciplineId equals discipline.Id
                                            join department in _context.Departments
                                            on discipline.DepartmentId equals department.Id
                                            where discipline.DepartmentId == departmentId
                                            select loadItem.HoursCovered;

        IQueryable<double> nonTeachingLoadQuery = from ntl in _context.NonTeachingLoad
                                                  join teacher in _context.Teachers
                                                  on ntl.TeacherId equals teacher.Id
                                                  where ntl.PeriodId == periodId && teacher.DepartmentId == departmentId
                                                  select ntl.Load;

        bool isPeriodFromCurrentYear = !await _periodsManager.IsPeriodInCurrentYear(periodId);

        double result = await loadItemsQuery.SumAsync() + await nonTeachingLoadQuery.SumAsync();

        return Math.Round(result, 2);
    }

    public async Task<double> GetDepartmentAverageTotalLoadCoveredInPeriodAsync(Guid periodId, Guid departmentId)
    {
        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        IQueryable<double> loadItemsQuery = from loadItem in _context.LoadItems
                                            join planItem in _context.TeachingPlanItems
                                            on loadItem.PlanningItemId equals planItem.Id
                                            where planItem.PeriodId == periodId
                                            join subject in _context.Subjects
                                            on planItem.SubjectId equals subject.Id
                                            join discipline in _context.Disciplines
                                            on subject.DisciplineId equals discipline.Id
                                            join department in _context.Departments
                                            on discipline.DepartmentId equals department.Id
                                            where discipline.DepartmentId == departmentId
                                            select loadItem.HoursCovered;

        IQueryable<double> nonTeachingLoadQuery = from ntl in _context.NonTeachingLoad
                                                  join teacher in _context.Teachers
                                                  on ntl.TeacherId equals teacher.Id
                                                  where ntl.PeriodId == periodId && teacher.DepartmentId == departmentId
                                                  select ntl.Load;

        bool isPeriodFromCurrentYear = !await _periodsManager.IsPeriodInCurrentYear(periodId);
        int teachersCount = await _context.Teachers.CountAsync(t => t.DepartmentId == departmentId && (!isPeriodFromCurrentYear || t.Active));

        double result = (await loadItemsQuery.SumAsync() + await nonTeachingLoadQuery.SumAsync()) / teachersCount;

        return Math.Round(result, 2);
    }

    public async Task<double> GetDepartmentTotalTimeFund(Guid departmentId, Guid periodId)
    {
        if (!await ExistDepartmentAsync(departmentId))
        {
            throw new DepartmentNotFoundException();
        }

        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        double periodTimeFund = await _periodsManager.GetPeriodTimeFund(periodId);
        int teachersCount = await GetDeparmentTeachersCountAsync(departmentId);
        return periodTimeFund * teachersCount;
    }

    public async Task<double> CalculateRAPAsync(Guid departmentId)
    {
        IQueryable<DisciplineModel> departmentDisciplines = from discipline in _context.Disciplines
                                                            where discipline.DepartmentId == departmentId
                                                            select discipline;

        IQueryable<SubjectModel> departmentSubjects = from subject in _context.Subjects
                                                      join discipline in _context.Disciplines
                                                      on subject.DisciplineId equals discipline.Id
                                                      select subject;

        IQueryable<TeachingPlanItemModel> planItems = from planItem in _context.TeachingPlanItems
                                                      join subject in departmentSubjects
                                                      on planItem.SubjectId equals subject.Id
                                                      select planItem;

        IQueryable<CourseModel> courses = from course in _context.Courses
                                          join planItem in planItems
                                          on course.Id equals planItem.CourseId
                                          join schoolYear in _context.SchoolYears
                                          on course.SchoolYearId equals schoolYear.Id
                                          where schoolYear.Current
                                          select course;

        courses = courses.Distinct().Include(c => c.Career);

        // Tiempo del departamento en la carreras de pregrado
        double departmentTimeAmount = await planItems.Where(item => !item.FromPostgraduateCourse).SumAsync(item => item.HoursPlanned);

        // Tiempo total de las asignaturas del pregrado que imparte el departamento en las carreras
        IQueryable<double> totalTimeSubjectsQuery = from periodSubject in _context.PeriodSubjects
                                                    join depSubject in departmentSubjects
                                                    on periodSubject.SubjectId equals depSubject.Id
                                                    join course in courses
                                                    on periodSubject.CourseId equals course.Id
                                                    join career in _context.Careers
                                                    on course.CareerId equals career.Id
                                                    where !career.PostgraduateCourse
                                                    select periodSubject.TotalHours;

        double totalTimeSubjects = await totalTimeSubjectsQuery.SumAsync();

        // Coeficiente de carga del pregrado
        double loadCoeff = departmentTimeAmount / totalTimeSubjects;

        // Matrícula de pregrado
        long enrolment = await courses.Where(c => !c.Career.PostgraduateCourse).SumAsync(c => c.Enrolment);

        // Matrícula equivalente de pregrado
        double equivEnrolment = enrolment * loadCoeff;

        // Tiempo del departamento en la carreras de posgrado
        double postgraduatedepartmentTimeAmount = await planItems.Where(item => item.FromPostgraduateCourse).SumAsync(item => item.HoursPlanned);

        // Tiempo total de las asignaturas del posgrado que imparte el departamento en las carreras
        IQueryable<double> postgraduatetotalTimeSubjectsQuery = from periodSubject in _context.PeriodSubjects
                                                                join depSubject in departmentSubjects
                                                                on periodSubject.SubjectId equals depSubject.Id
                                                                join course in courses
                                                                on periodSubject.CourseId equals course.Id
                                                                join career in _context.Careers
                                                                on course.CareerId equals career.Id
                                                                where career.PostgraduateCourse
                                                                select periodSubject.TotalHours;

        double postgraduatetotalTimeSubjects = await postgraduatetotalTimeSubjectsQuery.SumAsync();

        // Coeficiente de carga del posgrado
        double postgraduateloadCoeff = postgraduatedepartmentTimeAmount / postgraduatetotalTimeSubjects;

        // Matrícula de posgrado
        long postgraduateenrolment = await courses.Where(c => c.Career.PostgraduateCourse).SumAsync(c => c.Enrolment);

        // Matrícula equivalente de posgrado
        long postgraduateequivEnrolment = postgraduateenrolment * 3;

        // Matrícula equivalente total
        double totalEnrolment = equivEnrolment + postgraduateequivEnrolment;

        IQueryable<TeacherModel> teachers = from teacher in _context.Teachers
                                            where teacher.DepartmentId == departmentId && teacher.Active
                                            select teacher;

        return totalEnrolment / await teachers.CountAsync();
    }
}
