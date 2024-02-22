using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Services;

public class CoursesManager(QCUniversidadContext context,
                            ISchoolYearsManager schoolYearsManager,
                            ITeachersManager teachersManager,
                            ITeachersLoadManager teachersLoadManager,
                            IPeriodsManager periodsManager,
                            IOptions<CalculationOptions> calcOptions) : ICoursesManager
{
    private readonly QCUniversidadContext _context = context;
    private readonly ISchoolYearsManager _schoolYearsManager = schoolYearsManager;
    private readonly ITeachersManager _teachersManager = teachersManager;
    private readonly ITeachersLoadManager _teachersLoadManager = teachersLoadManager;
    private readonly IPeriodsManager _periodsManager = periodsManager;
    private readonly CalculationOptions _calculationOptions = calcOptions.Value;

    public async Task<CourseModel> CreateCourseAsync(CourseModel course)
    {
        ArgumentNullException.ThrowIfNull(course);

        await _context.Courses.AddAsync(course);
        int result = await _context.SaveChangesAsync();

        if (!course.LastCourse)
        {
            return course;
        }

        SchoolYearModel currentYear = await _schoolYearsManager.GetCurrentSchoolYearAsync();
        foreach (PeriodModel period in currentYear.Periods)
        {
            await _teachersLoadManager.RecalculateAllTeachersRelatedToCourseInPeriodAsync(course.Id, period.Id);
        }

        return course;
    }

    public async Task<bool> ExistsCourseAsync(Guid id)
    {
        bool result = await _context.Courses.AnyAsync(d => d.Id == id);
        return result;
    }

    public async Task<bool> CheckCourseExistenceByCareerYearAndModality(Guid careerId, int careerYear, TeachingModality modality)
    {
        bool result = await _context.Courses.AnyAsync(sy => sy.CareerId == careerId && sy.CareerYear == careerYear && sy.TeachingModality == modality);
        return result;
    }

    public async Task<IList<CourseModel>> GetCoursesAsync(int from, int to)
    {
        List<CourseModel> result =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.Courses.Skip(from).Take(to).Include(y => y.SchoolYear).Include(y => y.Career).Include(y => y.Curriculum).ToListAsync()
            : await _context.Courses.Include(y => y.SchoolYear).Include(y => y.Career).Include(y => y.Curriculum).ToListAsync();
        return result;
    }

    public async Task<IList<CourseModel>> GetCoursesAsync(Guid schoolYearId)
    {
        List<CourseModel> courses = await _context.Courses.Include(c => c.SchoolYear)
                                            .Include(c => c.Curriculum)
                                            .Where(c => c.SchoolYearId == schoolYearId)
                                            .ToListAsync();
        return courses;
    }

    public async Task<IList<CourseModel>> GetCoursesAsync(Guid schoolYearId, Guid facultyId)
    {
        IQueryable<CourseModel> coursesQuery = from course in _context.Courses
                                               join career in _context.Careers
                                               on course.CareerId equals career.Id
                                               join faculty in _context.Faculties
                                               on career.FacultyId equals faculty.Id
                                               where faculty.Id == facultyId && course.SchoolYearId == schoolYearId
                                               select course;
        List<CourseModel> courses = await coursesQuery.Include(c => c.SchoolYear)
                                        .Include(c => c.Curriculum)
                                        .Where(c => c.SchoolYearId == schoolYearId)
                                        .ToListAsync();
        return courses;
    }

    public async Task<IList<CourseModel>> GetCoursesAsync(Guid careerId, Guid schoolYearId, Guid facultyId)
    {
        IQueryable<CourseModel> coursesQuery = from course in _context.Courses
                                               join career in _context.Careers
                                               on course.CareerId equals career.Id
                                               join faculty in _context.Faculties
                                               on career.FacultyId equals faculty.Id
                                               where faculty.Id == facultyId
                                                     && course.SchoolYearId == schoolYearId
                                                     && course.CareerId == careerId
                                               select course;

        List<CourseModel> courses = await coursesQuery.Include(c => c.SchoolYear)
                                        .Include(c => c.Curriculum)
                                        .Where(c => c.SchoolYearId == schoolYearId)
                                        .ToListAsync();
        return courses;
    }

    public async Task<int> GetCoursesCountAsync()
        => await _context.Courses.CountAsync();

    public async Task<CourseModel> GetCourseAsync(Guid id)
    {
        CourseModel? result = await _context.Courses.Where(d => d.Id == id)
                                               .Include(y => y.SchoolYear)
                                               .Include(y => y.Career)
                                               .Include(y => y.Curriculum)
                                               .FirstOrDefaultAsync();
        return result ?? throw new CourseNotFoundException();
    }

    public async Task<bool> UpdateCourseAsync(CourseModel course)
    {
        ArgumentNullException.ThrowIfNull(course);

        _ = _context.Courses.Update(course);
        int result = await _context.SaveChangesAsync();

        if (result <= 0 || !course.LastCourse && !await CourseHavePlanningAsync(course.Id))
        {
            return result > 0;
        }

        SchoolYearModel currentYear = await _schoolYearsManager.GetCurrentSchoolYearAsync();
        foreach (PeriodModel period in currentYear.Periods)
        {
            await _teachersLoadManager.RecalculateAllTeachersRelatedToCourseInPeriodAsync(course.Id, period.Id);
        }

        return result > 0;
    }

    private async Task<bool> CourseHavePlanningAsync(Guid courseId)
    {
        return await _context.TeachingPlanItems.AnyAsync(p => p.CourseId == courseId);
    }

    public async Task<bool> DeleteCourseAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid course id", nameof(id));
        }

        try
        {
            CourseModel course = await GetCourseAsync(id);
            _ = _context.Courses.Remove(course);
            int result = await _context.SaveChangesAsync();

            if (result <= 0 || !course.LastCourse && !await CourseHavePlanningAsync(course.Id))
            {
                return result > 0;
            }

            SchoolYearModel currentYear = await _schoolYearsManager.GetCurrentSchoolYearAsync();
            foreach (PeriodModel period in currentYear.Periods)
            {
                await _teachersLoadManager.RecalculateAllTeachersRelatedToCourseInPeriodAsync(course.Id, period.Id);
            }

            return result > 0;
        }
        catch (DisciplineNotFoundException)
        {
            throw;
        }
    }

    public async Task<IList<CourseModel>> GetCoursesForDepartmentAsync(Guid departmentId, Guid? schoolYearId = null)
    {
        IQueryable<DisciplineModel> disciplines = from discipline in _context.Disciplines
                                                  where discipline.DepartmentId == departmentId
                                                  select discipline;
        IQueryable<CurriculumModel> curriculums = from curriculum in _context.Curriculums
                                                  join curriculumsDisciplines in _context.CurriculumsDisciplines
                                                  on curriculum.Id equals curriculumsDisciplines.CurriculumId
                                                  join discipline in disciplines
                                                  on curriculumsDisciplines.DisciplineId equals discipline.Id
                                                  select curriculum;
        IQueryable<CourseModel> courses = from course in _context.Courses
                                          join curriculum in curriculums
                                          on course.CurriculumId equals curriculum.Id
                                          select course;

        courses = courses.Where(c => _context.TeachingPlanItems.Any(planItem => planItem.CourseId == c.Id));

        if (schoolYearId.HasValue)
        {
            courses = courses.Where(c => c.SchoolYearId == schoolYearId);
        }

        courses = courses.Distinct();
        courses = courses.Include(y => y.SchoolYear)
                         .Include(sy => sy.Career)
                         .Include(sy => sy.Curriculum);
        return await courses.ToListAsync();
    }

    public async Task<double> GetHoursPlannedInPeriodForCourseAsync(Guid courseId, Guid periodId)
    {
        if (courseId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(courseId));
        }

        if (periodId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(periodId));
        }

        if (!await ExistsCourseAsync(courseId))
        {
            throw new CourseNotFoundException();
        }

        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        IQueryable<double> query = from psubject in _context.PeriodSubjects
                                   where psubject.PeriodId == periodId && psubject.CourseId == courseId
                                   select psubject.HoursPlanned;

        return (await query.ToListAsync()).Sum();
    }

    public async Task<double> GetTotalHoursInPeriodForCourseAsync(Guid courseId, Guid periodId)
    {
        if (courseId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(courseId));
        }

        if (periodId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(periodId));
        }

        if (!await ExistsCourseAsync(courseId))
        {
            throw new CourseNotFoundException();
        }

        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        IQueryable<double> query = from psubject in _context.PeriodSubjects
                                   where psubject.PeriodId == periodId && psubject.CourseId == courseId
                                   select psubject.TotalHours;

        return (await query.ToListAsync()).Sum();
    }

    public async Task<double> GetRealHoursPlannedInPeriodForCourseAsync(Guid courseId, Guid periodId)
    {
        if (courseId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(courseId));
        }

        if (periodId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(periodId));
        }

        if (!await ExistsCourseAsync(courseId))
        {
            throw new CourseNotFoundException();
        }

        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        IQueryable<double> query = from planItem in _context.TeachingPlanItems
                                   where planItem.PeriodId == periodId && planItem.CourseId == courseId
                                   select planItem.HoursPlanned;

        return (await query.ToListAsync()).Select(item => item * _calculationOptions.ClassHoursToRealHoursConversionCoefficient).Sum();
    }
}
