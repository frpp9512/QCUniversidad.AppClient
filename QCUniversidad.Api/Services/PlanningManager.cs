using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Notifications.Models;

namespace QCUniversidad.Api.Services;

public class PlanningManager(QCUniversidadContext context,
                             ICoefficientCalculator<TeachingPlanItemModel> planItemCalculator,
                             IDepartmentsManager departmentsManager,
                             IPeriodsManager periodsManager,
                             IMediator mediator) : IPlanningManager
{
    private readonly QCUniversidadContext _context = context;
    private readonly ICoefficientCalculator<TeachingPlanItemModel> _planItemCalculator = planItemCalculator;
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;
    private readonly IPeriodsManager _periodsManager = periodsManager;
    private readonly IMediator _mediator = mediator;

    public async Task<bool> CreateTeachingPlanItemAsync(TeachingPlanItemModel teachingPlanItem)
    {
        ArgumentNullException.ThrowIfNull(teachingPlanItem, nameof(teachingPlanItem));

        teachingPlanItem.FromPostgraduateCourse = await IsPostgraduateCourse(teachingPlanItem.CourseId);
        _ = await _context.TeachingPlanItems.AddAsync(teachingPlanItem);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    private async Task<bool> IsPostgraduateCourse(Guid courseId)
    {
        IQueryable<bool> query = from course in _context.Courses
                                 join career in _context.Careers
                                 on course.CareerId equals career.Id
                                 where course.Id == courseId
                                 select career.PostgraduateCourse;

        return await query.FirstAsync();
    }

    public async Task<bool> ExistsTeachingPlanItemAsync(Guid id)
    {
        bool result = await _context.TeachingPlanItems.AnyAsync(d => d.Id == id);
        return result;
    }

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(int from, int to)
    {
        List<TeachingPlanItemModel> result =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.TeachingPlanItems.Skip(from).Take(to).Include(p => p.Subject).Include(p => p.Course).ToListAsync()
            : await _context.TeachingPlanItems.Include(p => p.Subject).Include(p => p.Course).ToListAsync();
        result.ForEach(i => i.TotalHoursPlanned = _planItemCalculator.CalculateValue(i));
        return result;
    }

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(Guid periodId, Guid? courseId = null, int from = 0, int to = 0)
    {
        List<TeachingPlanItemModel> result =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.TeachingPlanItems.Where(tp => tp.PeriodId == periodId).Where(tp => courseId == null || tp.CourseId == courseId).Skip(from).Take(to).Include(p => p.Subject).Include(p => p.Course).Include(p => p.LoadItems).ThenInclude(i => i.Teacher).ToListAsync()
            : await _context.TeachingPlanItems.Where(tp => tp.PeriodId == periodId).Where(tp => courseId == null || tp.CourseId == courseId).Include(p => p.Subject).Include(p => p.Course).Include(p => p.LoadItems).ThenInclude(i => i.Teacher).ToListAsync();
        result.ForEach(i => i.TotalHoursPlanned = _planItemCalculator.CalculateValue(i));
        return result;
    }

    public async Task<int> GetTeachingPlanItemsCountAsync()
    {
        return await _context.TeachingPlanItems.CountAsync();
    }

    public async Task<int> GetTeachingPlanItemsCountAsync(Guid periodId)
    {
        return await _context.TeachingPlanItems.Where(tp => tp.PeriodId == periodId).CountAsync();
    }

    public async Task<TeachingPlanItemModel> GetTeachingPlanItemAsync(Guid id)
    {
        IQueryable<TeachingPlanItemModel> resultQuery = _context.TeachingPlanItems.Where(p => p.Id == id);
        TeachingPlanItemModel? result = await _context.TeachingPlanItems.Where(p => p.Id == id)
                                                     .Include(p => p.Subject)
                                                     .Include(p => p.LoadItems)
                                                        .ThenInclude(li => li.Teacher)
                                                     .Include(p => p.Course)
                                                     .FirstOrDefaultAsync() ?? throw new TeachingPlanItemNotFoundException();
        result.TotalHoursPlanned = _planItemCalculator.CalculateValue(result);
        return result;
    }

    public async Task<bool> UpdateTeachingPlanItemAsync(TeachingPlanItemModel teachingPlanItem)
    {
        ArgumentNullException.ThrowIfNull(teachingPlanItem);

        _ = _context.TeachingPlanItems.Update(teachingPlanItem);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteTeachingPlanItemAsync(Guid id)
    {
        try
        {
            TeachingPlanItemModel teachingPlanItem = await GetTeachingPlanItemAsync(id);
            var loadItemsAffected = await _context.LoadItems.Where(l => l.PlanningItemId == id)
                                                         .Select(l => new { teacherId = l.TeacherId, periodId = teachingPlanItem.PeriodId })
                                                         .ToListAsync();
            _ = _context.TeachingPlanItems.Remove(teachingPlanItem);
            int result = await _context.SaveChangesAsync();
            foreach (var item in loadItemsAffected)
            {
                await _mediator.Publish(new TeacherRecalculationRequested
                {
                    TeacherId = item.teacherId,
                    PeriodId = item.periodId
                });
            }

            return result > 0;
        }
        catch (TeachingPlanItemNotFoundException)
        {
            throw;
        }
    }

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsOfDepartmentOnPeriod(Guid departmentId, Guid periodId, Guid? courseId = null, bool onlyLoadItems = false)
    {
        try
        {
            if (!await _departmentsManager.ExistDepartmentAsync(departmentId))
            {
                throw new ArgumentException("Bad department id.", nameof(departmentId));
            }

            if (!await _periodsManager.ExistsPeriodAsync(periodId))
            {
                throw new ArgumentException("Bad period id.", nameof(periodId));
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
                                                          where planItem.PeriodId == periodId && (!onlyLoadItems || (onlyLoadItems && !planItem.IsNotLoadGenerator))
                                                          select planItem;

            IIncludableQueryable<TeachingPlanItemModel, TeacherModel?> items = planItems.Distinct().Include(i => i.Subject).Include(p => p.Course).Include(i => i.LoadItems).ThenInclude(li => li.Teacher);
            List<TeachingPlanItemModel> planItemsList = courseId is not null ? await items.Where(i => i.CourseId == courseId).ToListAsync() : await items.ToListAsync();
            planItemsList.ForEach(i => i.TotalHoursPlanned = _planItemCalculator.CalculateValue(i));
            return planItemsList;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> IsTeachingPlanFromPostgraduateCourse(Guid teachingPlanId)
    {
        if (teachingPlanId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(teachingPlanId));
        }

        if (!await ExistsTeachingPlanItemAsync(teachingPlanId))
        {
            throw new TeachingPlanItemNotFoundException();
        }
        IQueryable<bool> query = from planItem in _context.TeachingPlanItems
        join course in _context.Courses
                                 on planItem.CourseId equals course.Id
                                 join career in _context.Careers
                                 on course.CareerId equals career.Id
                                 where planItem.Id == teachingPlanId
                                 select career.PostgraduateCourse;

        return await query.FirstAsync();
    }

    public async Task<double> GetPlanItemTotalCoveredAsync(Guid planItemId)
    {
        if (planItemId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(planItemId));
        }

        IQueryable<LoadItemModel> query = from loadItem in _context.LoadItems
                                          where loadItem.PlanningItemId == planItemId
                                          select loadItem;
        return await query.SumAsync(i => i.HoursCovered);
    }
}
