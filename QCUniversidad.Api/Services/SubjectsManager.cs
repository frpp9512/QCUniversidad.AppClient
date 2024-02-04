using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Services;

public class SubjectsManager(QCUniversidadContext context,
                             IDisciplinesManager disciplinesManager,
                             ICoursesManager coursesManager,
                             IPeriodsManager periodsManager,
                             IOptions<CalculationOptions> calcOptions) : ISubjectsManager
{
    private readonly QCUniversidadContext _context = context;
    private readonly IDisciplinesManager _disciplinesManager = disciplinesManager;
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly IPeriodsManager _periodsManager = periodsManager;
    private readonly CalculationOptions _calculationOptions = calcOptions.Value;

    public async Task<bool> CreateSubjectAsync(SubjectModel subject)
    {
        if (subject is not null)
        {
            _ = await _context.Subjects.AddAsync(subject);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        throw new ArgumentNullException(nameof(subject));
    }

    public async Task<bool> ExistsSubjectAsync(Guid id)
    {
        bool result = await _context.Subjects.AnyAsync(t => t.Id == id);
        return result;
    }

    public async Task<bool> ExistsSubjectAsync(string name)
    {
        bool result = await _context.Subjects.AnyAsync(s => s.Name == name);
        return result;
    }

    public async Task<int> GetSubjectsCountAsync()
    {
        return await _context.Subjects.CountAsync();
    }

    public async Task<IList<SubjectModel>> GetSubjectsAsync(int from, int to)
    {
        List<SubjectModel> result =
            !(from == 0 && to == from)
            ? await _context.Subjects.Where(s => s.Active).Skip(from).Take(to).Include(d => d.Discipline).ToListAsync()
            : await _context.Subjects.Where(s => s.Active).Include(d => d.Discipline).ToListAsync();
        return result;
    }

    public async Task<IList<SubjectModel>> GetSubjectsForDisciplineAsync(Guid disciplineId)
    {
        if (disciplineId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(disciplineId));
        }

        if (!await _disciplinesManager.ExistsDisciplineAsync(disciplineId))
        {
            throw new DisciplineNotFoundException();
        }

        try
        {
            List<SubjectModel> subjects = await _context.Subjects.Where(s => s.DisciplineId == disciplineId).ToListAsync();
            return subjects;
        }
        catch
        {
            throw;
        }
    }

    public async Task<IList<SubjectModel>> GetSubjectsForCourseAsync(Guid courseId)
    {
        IQueryable<SubjectModel> query = from s in _context.Subjects
                                         join d in _context.Disciplines
                                         on s.DisciplineId equals d.Id
                                         join cd in _context.CurriculumsDisciplines
                                         on d.Id equals cd.DisciplineId
                                         join sy in _context.Courses
                                         on cd.CurriculumId equals sy.CurriculumId
                                         where sy.Id == courseId && s.Active
                                         select s;
        query = query.Include(s => s.Discipline);
        return await query.ToListAsync();
    }

    public async Task<IList<SubjectModel>> GetSubjectsForCourseInPeriodAsync(Guid courseId, Guid periodId)
    {
        if (!await _coursesManager.ExistsCourseAsync(courseId))
        {
            throw new CourseNotFoundException();
        }

        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        try
        {
            IQueryable<SubjectModel> query = from subject in _context.Subjects
                                             join periodSubject in _context.PeriodSubjects
                                             on subject.Id equals periodSubject.SubjectId
                                             where periodSubject.PeriodId == periodId && periodSubject.CourseId == courseId
                                             select subject;

            return await query.Include(s => s.Discipline).ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    // Update name so now it filters in career level no only for period.
    public async Task<IList<SubjectModel>> GetSubjectsForCourseNotAssignedInPeriodAsync(Guid courseId, Guid periodId)
    {
        IQueryable<SubjectModel> courseSubjectsQuery = from s in _context.Subjects
                                                       join d in _context.Disciplines
                                                       on s.DisciplineId equals d.Id
                                                       join cd in _context.CurriculumsDisciplines
                                                       on d.Id equals cd.DisciplineId
                                                       join sy in _context.Courses
                                                       on cd.CurriculumId equals sy.CurriculumId
                                                       where sy.Id == courseId && s.Active
                                                       select s;

        var courseInfo = await _context.Courses.Where(c => c.Id == courseId).Select(c => new { c.CareerId, c.TeachingModality, c.SchoolYearId, c.CurriculumId }).FirstAsync();
        IQueryable<Guid> relatedCoursesQuery = from course in _context.Courses
                                               where course.CareerId == courseInfo.CareerId
                                                  && course.TeachingModality == courseInfo.TeachingModality
                                                  && course.SchoolYearId == courseInfo.SchoolYearId
                                                  && course.CurriculumId == courseInfo.CurriculumId
                                               select course.Id;

        List<Guid> relatedCourses = await relatedCoursesQuery.ToListAsync();

        IQueryable<SubjectModel> assignedSubjectsQuery = from periodSubject in _context.PeriodSubjects
                                                         join subject in _context.Subjects
                                                         on periodSubject.SubjectId equals subject.Id
                                                         //where periodSubject.CourseId == courseId && periodSubject.PeriodId == periodId
                                                         where relatedCourses.Contains(periodSubject.CourseId)
                                                         select subject;

        IQueryable<SubjectModel> subjects = courseSubjectsQuery.Except(assignedSubjectsQuery);
        subjects = subjects.Include(s => s.Discipline);

        return await subjects.ToListAsync();
    }

    public async Task<SubjectModel> GetSubjectAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            SubjectModel? result = await _context.Subjects.Where(t => t.Id == id)
                                                .Include(s => s.Discipline)
                                                .FirstOrDefaultAsync();
            return result ?? throw new SubjectNotFoundException();
        }

        throw new ArgumentNullException(nameof(id));
    }

    public async Task<SubjectModel> GetSubjectAsync(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            SubjectModel? result = await _context.Subjects.Where(t => t.Name == name)
                                                .Include(s => s.Discipline)
                                                .FirstOrDefaultAsync();
            return result ?? throw new SubjectNotFoundException();
        }

        throw new ArgumentNullException(nameof(name));
    }

    public async Task<bool> UpdateSubjectAsync(SubjectModel subject)
    {
        if (subject is not null)
        {
            _ = _context.Subjects.Update(subject);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        throw new ArgumentNullException(nameof(subject));
    }

    public async Task<bool> DeleteSubjectAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            try
            {
                SubjectModel subject = await GetSubjectAsync(id);
                if (!await SubjectHaveLoad(id))
                {
                    _ = _context.Subjects.Remove(subject);
                }
                else
                {
                    subject.Active = false;
                    _ = _context.Update(subject);
                }

                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (SubjectNotFoundException)
            {
                throw;
            }
        }

        throw new ArgumentNullException(nameof(id));
    }

    private async Task<bool> SubjectHaveLoad(Guid subjectId)
    {
        if (subjectId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(subjectId));
        }

        if (!await ExistsSubjectAsync(subjectId))
        {
            throw new SubjectNotFoundException();
        }

        IQueryable<LoadItemModel> query = from planItem in _context.TeachingPlanItems
                                          join loadItem in _context.LoadItems
                                          on planItem.Id equals loadItem.PlanningItemId
                                          where planItem.SubjectId == subjectId
                                          select loadItem;

        return await query.CountAsync() > 0;
    }

    public async Task<IList<PeriodSubjectModel>> GetPeriodSubjectsForCourseAsync(Guid periodId, Guid courseId)
    {
        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        if (!await _coursesManager.ExistsCourseAsync(courseId))
        {
            throw new CourseNotFoundException();
        }

        IQueryable<PeriodSubjectModel> query = from periodSubject in _context.PeriodSubjects
                                               where periodSubject.PeriodId == periodId && periodSubject.CourseId == courseId
                                               select periodSubject;
        query = query.Include(ps => ps.Subject);

        return await query.ToListAsync();
    }

    public async Task<bool> CreatePeriodSubjectAsync(PeriodSubjectModel newPeriodSubject)
    {
        try
        {
            if (!await _periodsManager.ExistsPeriodAsync(newPeriodSubject.PeriodId))
            {
                throw new PeriodNotFoundException();
            }

            if (!await _coursesManager.ExistsCourseAsync(newPeriodSubject.CourseId))
            {
                throw new CourseNotFoundException();
            }

            if (!await ExistsSubjectAsync(newPeriodSubject.SubjectId))
            {
                throw new SubjectNotFoundException();
            }

            newPeriodSubject.TotalHours = newPeriodSubject.HoursPlanned * _calculationOptions.ClassHoursToRealHoursConversionCoefficient;
            _ = _context.PeriodSubjects.Add(newPeriodSubject);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PeriodSubjectModel> GetPeriodSubjectAsync(Guid id)
    {
        try
        {
            PeriodSubjectModel? result = await _context.PeriodSubjects.Where(ps => ps.Id == id)
                                                      .Include(ps => ps.Course)
                                                      .Include(ps => ps.Period)
                                                      .Include(ps => ps.Subject)
                                                      .FirstOrDefaultAsync();
            return result ?? throw new PeriodSubjectNotFoundException();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> ExistsPeriodSubjectAsync(Guid id)
    {
        try
        {
            return await _context.PeriodSubjects.AnyAsync(ps => ps.Id == id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> UpdatePeriodSubjectAsync(PeriodSubjectModel periodSubject)
    {
        if (periodSubject is null)
        {
            throw new ArgumentNullException(nameof(periodSubject));
        }

        try
        {
            periodSubject.TotalHours = periodSubject.HoursPlanned * _calculationOptions.ClassHoursToRealHoursConversionCoefficient;
            _ = _context.PeriodSubjects.Update(periodSubject);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeletePeriodSubjectAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (!await ExistsPeriodSubjectAsync(id))
        {
            throw new PeriodSubjectNotFoundException();
        }

        try
        {
            PeriodSubjectModel periodSubject = await _context.PeriodSubjects.FirstAsync(ps => ps.Id == id);
            _ = _context.PeriodSubjects.Remove(periodSubject);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
