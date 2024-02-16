using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.CommonModels;
using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.Api.Shared.Extensions;
using System.Text;

namespace QCUniversidad.Api.Services;

public class TeachersLoadManager : ITeachersLoadManager
{
    private readonly QCUniversidadContext _context;
    private readonly ITeachersManager _teachersManager;
    private readonly IPeriodsManager _periodsManager;
    private readonly IPlanningManager _planningManager;
    private readonly CalculationOptions _calculationOptions;

    public TeachersLoadManager(QCUniversidadContext context,
                               IOptions<CalculationOptions> calcOptions,
                               ITeachersManager teachersManager,
                               IPeriodsManager periodsManager,
                               IPlanningManager planningManager)
    {
        _context = context;
        _calculationOptions = calcOptions.Value;
        _planningManager = planningManager;
        _planningManager.RecalculationRequested += PlanningManager_RecalculationRequested;
        _periodsManager = periodsManager;
        _periodsManager.RecalculationRequested += PeriodsManager_RecalculationRequested;
        _teachersManager = teachersManager;
        _teachersManager.RequestLoadRecalculation += TeachersManager_RequestLoadRecalculation;
    }

    private async void PlanningManager_RecalculationRequested(object? sender, (Guid teacherId, Guid periodId) e)
    {
        await RecalculateAutogenerateTeachingLoadItemsAsync(e.teacherId, e.periodId);
    }

    private async void PeriodsManager_RecalculationRequested(object? sender, Guid e)
    {
        await RecalculateAllTeachersLoadInPeriodAsync(e);
    }

    private async void TeachersManager_RequestLoadRecalculation(object? sender, (Guid departmentId, Guid periodId) e)
    {
        await RecalculateAllTeachersLoadOfDepartmentInPeriodAsync(e.departmentId, e.periodId);
    }

    public async Task<IList<LoadItemModel>> GetTeacherLoadItemsInPeriodAsync(Guid teacherId, Guid periodId)
    {
        if (!await _teachersManager.ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }

        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        IQueryable<LoadItemModel> query = from loadItem in _context.LoadItems
                                          join planItem in _context.TeachingPlanItems
                                          on loadItem.PlanningItemId equals planItem.Id
                                          where loadItem.TeacherId == teacherId && planItem.PeriodId == periodId
                                          select loadItem;

        return await query.Include(l => l.PlanningItem)
                          .ThenInclude(p => p.Subject)
                          .Include(l => l.PlanningItem)
                          .ThenInclude(p => p.Course)
                          .Include(l => l.Teacher)
                          .ToListAsync();
    }

    public async Task<IList<NonTeachingLoadModel>> GetTeacherNonTeachingLoadItemsInPeriodAsync(Guid teacherId, Guid periodId)
    {
        if (!await _teachersManager.ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }

        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        List<NonTeachingLoadModel> items = await _context.NonTeachingLoad.Where(l => l.TeacherId == teacherId && l.PeriodId == periodId).ToListAsync();
        NonTeachingLoadType[] types = Enum.GetValues<NonTeachingLoadType>();
        foreach (NonTeachingLoadType type in types.Where(t => t.GetEnumDisplayAutogenerateValue()))
        {
            if (items.Any(i => i.Type == type))
            {
                continue;
            }

            try
            {
                NonTeachingLoadModel item = await RecalculateTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
                items.Add(item);
            }
            catch
            {
                continue;
            }
        }

        return items;
    }

    public async Task<NonTeachingLoadModel> GetTeacherNonTeachingLoadItemInPeriodAsync(NonTeachingLoadType type, Guid teacherId, Guid periodId)
    {
        if (!await _teachersManager.ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }

        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        IQueryable<NonTeachingLoadModel> itemQuery = from ntl in _context.NonTeachingLoad
                                                     where ntl.TeacherId == teacherId && ntl.PeriodId == periodId && ntl.Type == type
                                                     select ntl;
        return await itemQuery.FirstOrDefaultAsync();
    }

    public async Task<NonTeachingLoadModel> RecalculateTeacherNonTeachingLoadItemInPeriodAsync(NonTeachingLoadType type, Guid teacherId, Guid periodId)
    {
        NonTeachingLoadModel? loadItem = await GetTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);

        if (!type.GetEnumDisplayAutogenerateValue())
        {
            if (loadItem is not null)
            {
                _ = await SetNonTeachingLoadAsync(loadItem.Type, loadItem.BaseValue, teacherId, periodId);
            }

            return null;
        }

        NonTeachingLoadModel calculated = await CalculateNonTeachingLoadOfTypeAsync(type, teacherId, periodId);
        if (loadItem is not null)
        {
            loadItem.Load = calculated.Load;
            loadItem.BaseValue = calculated.BaseValue;
            loadItem.Description = calculated.Description;
            _ = _context.NonTeachingLoad.Update(loadItem);
        }
        else
        {
            _ = _context.NonTeachingLoad.Add(calculated);
        }

        int result = await _context.SaveChangesAsync();
        return result > 0 ? (loadItem is null ? calculated : loadItem) : throw new DatabaseOperationException();
    }

    public async Task RecalculateAllTeachersLoadInPeriodAsync(Guid periodId)
    {
        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        if (!await _periodsManager.IsPeriodInCurrentYear(periodId))
        {
            return;
        }

        IQueryable<Guid> teachersQuery = from teacher in _context.Teachers
                                         where teacher.Active
                                         select teacher.Id;
        List<Guid> teachersId = await teachersQuery.ToListAsync();
        foreach (NonTeachingLoadType type in Enum.GetValues<NonTeachingLoadType>())
        {
            foreach (Guid teacherId in teachersId)
            {
                _ = await RecalculateTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
            }
        }
    }

    public async Task RecalculateAllTeachersLoadOfDepartmentInPeriodAsync(Guid departmentId, Guid periodId)
    {
        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        if (!await _periodsManager.IsPeriodInCurrentYear(periodId))
        {
            return;
        }

        IQueryable<Guid> teachersQuery = from teacher in _context.Teachers
                                         where teacher.Active && teacher.DepartmentId == departmentId
                                         select teacher.Id;
        List<Guid> teachersId = await teachersQuery.ToListAsync();
        foreach (NonTeachingLoadType type in Enum.GetValues<NonTeachingLoadType>())
        {
            foreach (Guid teacherId in teachersId)
            {
                _ = await RecalculateTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
            }
        }
    }

    public async Task RecalculateAllTeachersRelatedToCourseInPeriodAsync(Guid courseId, Guid periodId)
    {
        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        if (!await _periodsManager.IsPeriodInCurrentYear(periodId))
        {
            return;
        }

        IQueryable<Guid> query = from department in _context.Departments
                                 join departmentCareer in _context.DepartmentsCareers
                                 on department.Id equals departmentCareer.DepartmentId
                                 join career in _context.Careers
                                 on departmentCareer.CareerId equals career.Id
                                 join course in _context.Courses
                                 on career.Id equals course.CareerId
                                 where course.Id == courseId
                                 select department.Id;
        foreach (Guid departmentId in await query.ToListAsync())
        {
            await RecalculateAllTeachersLoadOfDepartmentInPeriodAsync(departmentId, periodId);
        }
    }

    private async Task<NonTeachingLoadModel> CalculateNonTeachingLoadOfTypeAsync(NonTeachingLoadType type, Guid teacherId, Guid periodId)
    {
        TeacherModel teacher = await _teachersManager.GetTeacherAsync(teacherId);
        bool readjustedByContract = false;
        double monthCount = 0;

        Func<Task<double>> getMonthCount = new(async () =>
        {
            IQueryable<double> monthCountQuery = from period in _context.Periods
                                                 where period.Id == periodId
                                                 select period.MonthsCount;
            return await monthCountQuery.FirstAsync();
        });

        switch (type)
        {
            case NonTeachingLoadType.Consultation:
                return CalculateConsultation(teacherId, periodId);
            case NonTeachingLoadType.ClassPreparation:
                return CalculateClassPreparation(teacherId, periodId);
            case NonTeachingLoadType.Meetings:
                monthCount = await getMonthCount();

                double mValue = _calculationOptions.MeetingsCoefficient;
                if (teacher.ContractType == TeacherContractType.PartTime)
                {
                    mValue = mValue * teacher.SpecificTimeFund / _calculationOptions.MonthTimeFund;
                    readjustedByContract = true;
                }

                NonTeachingLoadModel mItem = new()
                {
                    Type = NonTeachingLoadType.Meetings,
                    Description = $"{Math.Round(mValue, 2)} horas al mes{(readjustedByContract ? " (Reajustado por tipo de contrato)" : "")} x {monthCount} meses en el período - {NonTeachingLoadType.Meetings.GetEnumDisplayDescriptionValue()}",
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(monthCount),
                    Load = Math.Round(monthCount * mValue, 2)
                };
                return mItem;
            case NonTeachingLoadType.MethodologicalActions:
                monthCount = await getMonthCount();

                double maValue = _calculationOptions.MethodologicalActionsCoefficient;
                if (teacher.ContractType == TeacherContractType.PartTime)
                {
                    maValue = maValue * teacher.SpecificTimeFund / _calculationOptions.MonthTimeFund;
                    readjustedByContract = true;
                }

                NonTeachingLoadModel mtItem = new()
                {
                    Type = NonTeachingLoadType.MethodologicalActions,
                    Description = $"{Math.Round(maValue, 2)} horas al mes{(readjustedByContract ? " (Reajustado por tipo de contrato)" : "")} x {monthCount} meses en el período - {NonTeachingLoadType.MethodologicalActions.GetEnumDisplayDescriptionValue()}",
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(monthCount),
                    Load = Math.Round(monthCount * maValue, 2)
                };
                return mtItem;
            case NonTeachingLoadType.EventsAndPublications:
                monthCount = await getMonthCount();

                double eValue = _calculationOptions.EventsAndPublicationsCoefficient;

                bool departmentIsStudyCenter = await _context.Departments.Where(d => d.Id == teacher.DepartmentId).Select(d => d.IsStudyCenter).FirstAsync();
                if (departmentIsStudyCenter)
                {
                    eValue *= 3;
                }

                if (teacher.ContractType == TeacherContractType.PartTime)
                {
                    eValue = eValue * teacher.SpecificTimeFund / _calculationOptions.MonthTimeFund;
                    readjustedByContract = true;
                }

                NonTeachingLoadModel eItem = new()
                {
                    Type = NonTeachingLoadType.EventsAndPublications,
                    Description = $"{Math.Round(eValue, 2)} horas al mes{(departmentIsStudyCenter ? " (Pertenece a centro de estudio)" : "")}{(readjustedByContract ? " (Reajustado por tipo de contrato)" : "")} x {monthCount} meses en el período - {NonTeachingLoadType.EventsAndPublications.GetEnumDisplayDescriptionValue()}",
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(monthCount),
                    Load = Math.Round(monthCount * eValue, 2)
                };
                return eItem;
            case NonTeachingLoadType.OtherActivities:
                monthCount = await getMonthCount();

                double oaValue = _calculationOptions.OtherActivitiesCoefficient;
                if (teacher.ContractType == TeacherContractType.PartTime)
                {
                    oaValue = oaValue * teacher.SpecificTimeFund / _calculationOptions.MonthTimeFund;
                    readjustedByContract = true;
                }

                NonTeachingLoadModel oaItem = new()
                {
                    Type = NonTeachingLoadType.OtherActivities,
                    Description = $"{Math.Round(oaValue, 2)} horas al mes{(readjustedByContract ? " (Reajustado por tipo de contrato)" : "")} x {monthCount} meses en el período - {NonTeachingLoadType.OtherActivities.GetEnumDisplayDescriptionValue()}",
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(monthCount),
                    Load = Math.Round(monthCount * oaValue, 2)
                };
                return oaItem;
            case NonTeachingLoadType.ExamGrade:
                var loadSubjects = from loadItem in _context.LoadItems
                                   join planItem in _context.TeachingPlanItems
                                   on loadItem.PlanningItemId equals planItem.Id
                                   where loadItem.TeacherId == teacherId && planItem.PeriodId == periodId
                                   select new { planItem.SubjectId, planItem.CourseId };

                double examGradeTotal = 0;

                double midTermExamGradeTotal = 0;
                double finalExamGradeTotal = 0;
                double secondFinalExamGradeTotal = 0;
                double thirdFinalExamGradeTotal = 0;
                double courseWorkTotal = 0;
                double secondCourseWorkTotal = 0;
                double thirdCourseWorkTotal = 0;

                var loadSubjectsInfo = await loadSubjects.Distinct().ToListAsync();
                var loadSubjectsGroup = loadSubjectsInfo.GroupBy(ls => ls.CourseId);

                foreach (var lsc in loadSubjectsGroup)
                {
                    Guid currentCourse = lsc.First().CourseId;
                    IQueryable<uint> courseEnrolmentQuery = from course in _context.Courses
                                                            where course.Id == currentCourse
                                                            select course.Enrolment;
                    uint courseEnrolment = await courseEnrolmentQuery.FirstAsync();

                    double subtotal = 0;

                    foreach (var ls in lsc)
                    {
                        var periodSubjectDataQuery = from periodSubject in _context.PeriodSubjects
                                                     where periodSubject.PeriodId == periodId && periodSubject.CourseId == ls.CourseId && periodSubject.SubjectId == ls.SubjectId
                                                     select new { periodSubject.MidtermExamsCount, periodSubject.TerminationMode };

                        var periodSubjectData = await periodSubjectDataQuery.FirstAsync();

                        IQueryable<Guid> teachersCountQuery = from loadItem in _context.LoadItems
                                                              join planItem in _context.TeachingPlanItems
                                                              on loadItem.PlanningItemId equals planItem.Id
                                                              where planItem.PeriodId == periodId && planItem.SubjectId == ls.SubjectId && planItem.CourseId == ls.CourseId
                                                              select loadItem.TeacherId;
                        int teachersCount = await teachersCountQuery.Distinct().CountAsync();

                        double midTermExamValue = periodSubjectData.MidtermExamsCount * _calculationOptions.ExamGradeMidTermAverageTime * courseEnrolment / teachersCount;
                        midTermExamGradeTotal += periodSubjectData.MidtermExamsCount * courseEnrolment;

                        double terminationValue = 0;

                        switch (periodSubjectData.TerminationMode)
                        {
                            case SubjectTerminationMode.FinalExam:
                                double finalExamValue = _calculationOptions.ExamGradeFinalAverageTime * (courseEnrolment * _calculationOptions.ExamGradeFinalCoefficient);
                                finalExamGradeTotal += courseEnrolment * _calculationOptions.ExamGradeFinalCoefficient;

                                double secondFinalExamValue = _calculationOptions.ExamGradeFinalAverageTime * (courseEnrolment * _calculationOptions.SecondExamGradeFinalCoefficient);
                                secondFinalExamGradeTotal += courseEnrolment * _calculationOptions.SecondExamGradeFinalCoefficient;

                                double thirdFinalExamValue = _calculationOptions.ExamGradeFinalAverageTime * (courseEnrolment * _calculationOptions.ThirdExamGradeFinalCoefficient);
                                thirdFinalExamGradeTotal += courseEnrolment * _calculationOptions.ThirdExamGradeFinalCoefficient;

                                terminationValue = finalExamValue + secondFinalExamValue + thirdFinalExamValue;
                                break;
                            case SubjectTerminationMode.CourseWork:
                                double courseWorkValue = _calculationOptions.CourseWorkAverageTime * (courseEnrolment * _calculationOptions.CourseWorkEnrolmentCoefficient / _calculationOptions.CourseWorkEnrolmentDivider);
                                courseWorkTotal += courseEnrolment * _calculationOptions.CourseWorkEnrolmentCoefficient / _calculationOptions.CourseWorkEnrolmentDivider;

                                double secondCourseWorkValue = _calculationOptions.CourseWorkAverageTime * (courseEnrolment * _calculationOptions.SecondCourseWorkEnrolmentCoefficient / _calculationOptions.CourseWorkEnrolmentDivider);
                                secondCourseWorkTotal += courseEnrolment * _calculationOptions.SecondCourseWorkEnrolmentCoefficient / _calculationOptions.CourseWorkEnrolmentDivider;

                                double thirdCourseWorkValue = _calculationOptions.CourseWorkAverageTime * (courseEnrolment * _calculationOptions.ThirdCourseWorkEnrolmentCoefficient / _calculationOptions.CourseWorkEnrolmentDivider);
                                thirdCourseWorkTotal += courseEnrolment * _calculationOptions.ThirdCourseWorkEnrolmentCoefficient / _calculationOptions.CourseWorkEnrolmentDivider;

                                terminationValue = courseWorkValue + secondCourseWorkValue + thirdCourseWorkValue;
                                break;
                            case SubjectTerminationMode.AcademicHistory:
                                break;
                            default:
                                break;
                        }

                        double examGradeValue = (midTermExamValue + terminationValue) / teachersCount;
                        subtotal += examGradeValue;
                    }

                    examGradeTotal += subtotal;
                }

                StringBuilder examGradeDescriptionBuilder = new();

                if (midTermExamGradeTotal > 0)
                {
                    _ = examGradeDescriptionBuilder.AppendLine($"{Math.Round(midTermExamGradeTotal, 2)} exámenes parciales x {_calculationOptions.ExamGradeMidTermAverageTime} horas cada uno.");
                }

                if (finalExamGradeTotal > 0)
                {
                    _ = examGradeDescriptionBuilder.AppendLine($"{Math.Round(finalExamGradeTotal, 2)} exámenes finales x {_calculationOptions.ExamGradeFinalAverageTime} horas cada uno.");
                    _ = examGradeDescriptionBuilder.AppendLine($"{Math.Round(secondFinalExamGradeTotal, 2)} exámenes extraordinarios x {_calculationOptions.ExamGradeFinalAverageTime} horas cada uno.");
                    _ = examGradeDescriptionBuilder.AppendLine($"{Math.Round(thirdFinalExamGradeTotal, 2)} exámenes mundiales x {_calculationOptions.ExamGradeFinalAverageTime} horas cada uno.");
                }

                if (courseWorkTotal > 0)
                {
                    _ = examGradeDescriptionBuilder.AppendLine($"{Math.Round(courseWorkTotal)} trabajos de curso en ordinario x {_calculationOptions.CourseWorkAverageTime} horas cada uno.");
                    _ = examGradeDescriptionBuilder.AppendLine($"{Math.Round(secondCourseWorkTotal)} trabajos de curso en ordinario x {_calculationOptions.CourseWorkAverageTime} horas cada uno.");
                    _ = examGradeDescriptionBuilder.AppendLine($"{Math.Round(thirdCourseWorkTotal)} trabajos de curso en ordinario x {_calculationOptions.CourseWorkAverageTime} horas cada uno.");
                }

                if (midTermExamGradeTotal == 0 && finalExamGradeTotal == 0 && courseWorkTotal == 0)
                {
                    _ = examGradeDescriptionBuilder.AppendLine(NonTeachingLoadType.ExamGrade.GetEnumDisplayNameValue());
                }

                NonTeachingLoadModel examGradeLoad = new()
                {
                    Type = NonTeachingLoadType.ExamGrade,
                    Description = examGradeDescriptionBuilder.ToString(),
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(examGradeTotal),
                    Load = Math.Round(examGradeTotal, 2)
                };
                return examGradeLoad;
            case NonTeachingLoadType.ThesisCourtAndRevision:
                if (!await _periodsManager.IsLastPeriodAsync(periodId))
                {
                    return new NonTeachingLoadModel
                    {
                        Type = NonTeachingLoadType.ThesisCourtAndRevision,
                        Description = $"Esta carga se calcula solamente en el último período del año",
                        TeacherId = teacherId,
                        PeriodId = periodId,
                        BaseValue = JsonConvert.SerializeObject(0),
                        Load = 0
                    };
                }

                Guid teacherDepartmentId = await _context.Teachers.Where(t => t.Id == teacherId).Select(t => t.DepartmentId).FirstAsync();
                int teacherDepartmentCount = await _context.Teachers.CountAsync(t => t.DepartmentId == teacherDepartmentId && t.Active);
                IQueryable<Guid> schoolYearIdQuery = from period in _context.Periods
                                                     join schoolYear in _context.SchoolYears
                                                     on period.SchoolYearId equals schoolYear.Id
                                                     where period.Id == periodId
                                                     select schoolYear.Id;
                Guid schoolYearId = await schoolYearIdQuery.FirstAsync();
                IQueryable<uint> finalCoursesEnrolmentQuery = from course in _context.Courses
                                                              where course.SchoolYearId == schoolYearId && course.LastCourse
                                                              join career in _context.Careers
                                                              on course.CareerId equals career.Id
                                                              join departmentCareer in _context.DepartmentsCareers
                                                              on career.Id equals departmentCareer.CareerId
                                                              where departmentCareer.DepartmentId == teacherDepartmentId
                                                              select course.Enrolment;
                List<uint> finalCoursesEnrolmentResult = await finalCoursesEnrolmentQuery.ToListAsync();
                double finalCoursesEnrolment = finalCoursesEnrolmentResult.Select(ce => (double)ce).Sum();
                double thesisCourtTotal = finalCoursesEnrolment * _calculationOptions.ThesisCourtCountMultiplier / teacherDepartmentCount;
                double thesisCourtLoadValue = thesisCourtTotal * _calculationOptions.ThesisCourtCoefficient;
                NonTeachingLoadModel thesisCourtLoad = new()
                {
                    Type = NonTeachingLoadType.ThesisCourtAndRevision,
                    Description = $"{Math.Round(thesisCourtTotal, 2)} tribunales de pregrado de un total de {finalCoursesEnrolment}.",
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(finalCoursesEnrolmentQuery),
                    Load = Math.Round(thesisCourtLoadValue, 2)
                };
                return thesisCourtLoad;
        }

        return null;
    }

    private NonTeachingLoadModel CalculateClassPreparation(Guid teacherId, Guid periodId)
    {
        var cpQuery = from loadItem in _context.LoadItems
                      join planItem in _context.TeachingPlanItems
                      on loadItem.PlanningItemId equals planItem.Id
                      where loadItem.TeacherId == teacherId
                            && planItem.PeriodId == periodId
                      select new { hoursCovered = loadItem.HoursCovered, type = planItem.Type, groupCount = planItem.GroupsAmount };

        ClassPreparationCalculationModel calculationModel = new();
        double primaryGroups = 0;
        int primaryItemsCounted = 0;
        double secondaryGroups = 0;
        int secondaryItemsCounted = 0;
        double tertiaryGroups = 0;
        int tertiaryItemsCounted = 0;

        foreach (var value in cpQuery)
        {
            if (value.type is TeachingActivityType.Conference or TeachingActivityType.PostgraduateClass)
            {
                primaryGroups += value.groupCount;
                primaryItemsCounted++;
                calculationModel.MainClassesValue += value.hoursCovered / value.groupCount;
                continue;
            }

            if (value.type == TeachingActivityType.MeetingClass)
            {
                secondaryGroups += value.groupCount;
                secondaryItemsCounted++;
                calculationModel.SecondaryClassesValue += value.hoursCovered / value.groupCount;
                continue;
            }

            tertiaryGroups += value.groupCount;
            tertiaryItemsCounted++;
            calculationModel.TertiaryClassesValue += value.hoursCovered / value.groupCount;
        }

        if (primaryGroups > 0)
        {
            calculationModel.MainClassesGroupCount = primaryGroups / primaryItemsCounted;
        }

        if (secondaryGroups > 0)
        {
            calculationModel.SecondaryClassesGroupCount = secondaryGroups / secondaryItemsCounted;
        }

        if (tertiaryGroups > 0)
        {
            calculationModel.TertiaryClassesGroupCount = tertiaryGroups / tertiaryItemsCounted;
        }

        StringBuilder descriptionBuilder = new();
        if (calculationModel.MainClassesValue > 0)
        {
            _ = descriptionBuilder.AppendLine($"{calculationModel.MainClassesValue} conferencias o clases a postgrado x {_calculationOptions.ClassPreparationPrimaryCoefficient} horas de preparación cada una, dividido entre {calculationModel.MainClassesGroupCount} grupos.");
        }

        if (calculationModel.SecondaryClassesValue > 0)
        {
            _ = descriptionBuilder.AppendLine($"{calculationModel.SecondaryClassesValue} clases encuentro x {_calculationOptions.ClassPreparationSecondaryCoefficient} horas de preparación cada una, dividido entre {calculationModel.SecondaryClassesGroupCount} grupos.");
        }

        if (calculationModel.TertiaryClassesValue > 0)
        {
            _ = descriptionBuilder.AppendLine($"{calculationModel.SecondaryClassesValue} de otras actividades docentes x {_calculationOptions.ClassPreparationTertiaryCoefficient} horas de preparación cada una, dividido entre {calculationModel.TertiaryClassesGroupCount} grupos.");
        }

        if (calculationModel.MainClassesValue == 0 && calculationModel.SecondaryClassesValue == 0 && calculationModel.TertiaryClassesValue == 0)
        {
            _ = descriptionBuilder.AppendLine(NonTeachingLoadType.ClassPreparation.GetEnumDisplayDescriptionValue());
        }

        NonTeachingLoadModel cpItem = new()
        {
            Type = NonTeachingLoadType.ClassPreparation,
            Description = descriptionBuilder.ToString(),
            TeacherId = teacherId,
            PeriodId = periodId,
            BaseValue = JsonConvert.SerializeObject(calculationModel),
            Load = Math.Round(
                (calculationModel.MainClassesValue * _calculationOptions.ClassPreparationPrimaryCoefficient)
                + (calculationModel.SecondaryClassesValue * _calculationOptions.ClassPreparationSecondaryCoefficient)
                + (calculationModel.TertiaryClassesValue * _calculationOptions.ClassPreparationTertiaryCoefficient),
                2)
        };
        return cpItem;
    }

    private NonTeachingLoadModel CalculateConsultation(Guid teacherId, Guid periodId)
    {
        var cQuery = from loadItem in _context.LoadItems
                     join planItem in _context.TeachingPlanItems
                     on loadItem.PlanningItemId equals planItem.Id
                     where loadItem.TeacherId == teacherId
                           && planItem.PeriodId == periodId
                           && !planItem.FromPostgraduateCourse
                     select new { planItem.SubjectId, planItem.CourseId };
        int cValue = cQuery.Distinct().Count();
        string description = NonTeachingLoadType.Consultation.GetEnumDisplayDescriptionValue();
        if (cValue > 0)
        {
            description = $"{cValue} asignaturas x {_calculationOptions.ConsultationCoefficient} horas de consultas a cada una.";
        }

        NonTeachingLoadModel cItem = new()
        {
            Type = NonTeachingLoadType.Consultation,
            Description = description,
            TeacherId = teacherId,
            PeriodId = periodId,
            BaseValue = JsonConvert.SerializeObject(cValue),
            Load = Math.Round(cValue * _calculationOptions.ConsultationCoefficient, 2)
        };
        return cItem;
    }

    public async Task<double> GetTeacherLoadInPeriodAsync(Guid teacherId, Guid periodId)
    {
        if (teacherId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(teacherId));
        }

        if (!await _teachersManager.ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }

        if (periodId == Guid.Empty)
        {
            throw new ArgumentException(null, nameof(periodId));
        }

        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        IQueryable<LoadItemModel> query = from loadItem in _context.LoadItems
                                          join planItem in _context.TeachingPlanItems
                                          on loadItem.PlanningItemId equals planItem.Id
                                          where loadItem.TeacherId == teacherId && planItem.PeriodId == periodId
                                          select loadItem;

        IQueryable<double> nonTeachingLoadQuery = from ntl in _context.NonTeachingLoad
                                                  where ntl.TeacherId == teacherId && ntl.PeriodId == periodId
                                                  select ntl.Load;

        double nonTeachingValue = await _context.NonTeachingLoad.Where(item => item.TeacherId == teacherId && item.PeriodId == periodId).SumAsync(item => item.Load);
        if (nonTeachingValue == 0)
        {
            await RecalculateAutogenerateTeachingLoadItemsAsync(teacherId, periodId);
            nonTeachingValue = await _context.NonTeachingLoad.Where(item => item.TeacherId == teacherId && item.PeriodId == periodId).SumAsync(item => item.Load);
        }

        return Math.Round(await query.SumAsync(i => i.HoursCovered) + nonTeachingValue, 2);
    }

    public async Task RecalculateAutogenerateTeachingLoadItemsAsync(Guid teacherId, Guid periodId)
    {
        IEnumerable<NonTeachingLoadType> types = Enum.GetValues<NonTeachingLoadType>().Where(type => type.GetEnumDisplayAutogenerateValue());
        foreach (NonTeachingLoadType type in types)
        {
            try
            {
                _ = await RecalculateTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
            }
            catch { }
        }
    }

    private async Task RecalculateNonTeachingLoadItemsAsync(Guid teacherId, Guid periodId)
    {
        IEnumerable<NonTeachingLoadType> recalculableTypes = Enum.GetValues<NonTeachingLoadType>().Where(type => type.IsRecalculable());
        foreach (NonTeachingLoadType type in recalculableTypes)
        {
            try
            {
                _ = await RecalculateTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
            }
            catch { }
        }
    }

    public async Task<IList<TeacherModel>> GetTeachersOfDepartmentNotAssignedToPlanItemAsync(Guid departmentId, Guid planItemId, Guid? disciplineId = null)
    {
        if (departmentId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(departmentId));
        }

        if (planItemId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(planItemId));
        }

        IQueryable<TeacherModel> depTeachersQuery = from teacher in _context.Teachers
                                                    where teacher.Active
                                                    join teacherDiscipline in _context.TeachersDisciplines
                                                    on teacher.Id equals teacherDiscipline.TeacherId
                                                    where teacherDiscipline.DisciplineId == disciplineId
                                                       && (teacher.DepartmentId == departmentId || teacher.ServiceProvider)
                                                    select teacher;

        depTeachersQuery = depTeachersQuery.Distinct();

        IQueryable<LoadItemModel> planItemLoads = from planItem in _context.TeachingPlanItems
                                                  join loadItem in _context.LoadItems
                                                  on planItem.Id equals loadItem.PlanningItemId
                                                  where planItem.Id == planItemId
                                                  select loadItem;

        IQueryable<TeacherModel> depTeachersInPlanItemQuery = from teacher in _context.Teachers
                                                              join loadItem in planItemLoads
                                                              on teacher.Id equals loadItem.TeacherId
                                                              where teacher.Active
                                                              select teacher;

        var finalQuery = depTeachersQuery.Except(depTeachersInPlanItemQuery).Where(t => t.Active).Include(t => t.Department);
        return await finalQuery.ToListAsync();
    }

    public async Task<bool> SetLoadToTeacher(Guid teacherId, Guid planItemId, double hours)
    {
        if (teacherId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(teacherId));
        }

        if (planItemId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(planItemId));
        }

        if (!await _teachersManager.ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }

        if (!await _planningManager.ExistsTeachingPlanItemAsync(planItemId))
        {
            throw new TeachingPlanItemNotFoundException();
        }

        if (hours <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hours), "The hours amount should be greater than zero.");
        }

        TeachingPlanItemModel planItem = await _planningManager.GetTeachingPlanItemAsync(planItemId);
        double hoursToCover = planItem.TotalHoursPlanned - planItem.LoadItems.Sum(i => i.HoursCovered);
        if (hoursToCover <= 0)
        {
            throw new PlanItemFullyCoveredException();
        }

        LoadItemModel loadItem = new()
        {
            TeacherId = teacherId,
            PlanningItemId = planItemId,
            HoursCovered = hours > hoursToCover ? hoursToCover : hours
        };
        _ = await _context.AddAsync(loadItem);
        int result = await _context.SaveChangesAsync();
        await RecalculateNonTeachingLoadItemsAsync(teacherId, planItem.PeriodId);
        return result > 0;
    }

    public async Task<bool> DeleteLoadFromTeacherAsync(Guid loadItemId)
    {
        if (!await ExistsLoadItemAsync(loadItemId))
        {
            throw new LoadItemNotFoundException();
        }

        LoadItemModel? loadItem = await _context.LoadItems.Where(l => l.Id == loadItemId).Include(l => l.PlanningItem).FirstOrDefaultAsync();
        if (loadItem is null)
        {
            return false;
        }

        _ = _context.Remove(loadItem);
        int result = await _context.SaveChangesAsync();
        await RecalculateNonTeachingLoadItemsAsync(loadItem.TeacherId, loadItem.PlanningItem.PeriodId);
        return result > 0;
    }

    private async Task<bool> ExistsLoadItemAsync(Guid loadItemId) => await _context.LoadItems.AnyAsync(i => i.Id == loadItemId);

    public async Task<bool> SetNonTeachingLoadAsync(NonTeachingLoadType type, string baseValue, Guid teacherId, Guid periodId)
    {
        if (!await _teachersManager.ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }

        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        if (string.IsNullOrEmpty(baseValue))
        {
            throw new ArgumentNullException(nameof(baseValue));
        }

        TeacherModel teacher = await _teachersManager.GetTeacherAsync(teacherId);
        return type switch
        {
            NonTeachingLoadType.PostgraduateThesisCourtAndRevision => await CalculatePostgraduateThesisCourtAndRevision(type, baseValue, teacherId, periodId),
            NonTeachingLoadType.CoursesReceivedAndImprovement => await CalculateCoursesReceivedAndImprovement(type, baseValue, teacherId, periodId, teacher),
            NonTeachingLoadType.UndergraduateTutoring => await CalculateUndergraduateTutoring(type, baseValue, teacherId, periodId),
            NonTeachingLoadType.GraduateTutoring => await CalculateGraduateTutoring(type, baseValue, teacherId, periodId),
            NonTeachingLoadType.ParticipationInProjects => await CalculateParticipationInProjects(type, baseValue, teacherId, periodId, teacher),
            NonTeachingLoadType.UniversityExtensionActions => await CalculateUniversitaryExtensionActions(type, baseValue, teacherId, periodId, teacher),
            NonTeachingLoadType.EducationalWork => await CalculateEducationalWork(type, baseValue, teacherId, periodId, teacher),
            NonTeachingLoadType.AdministrativeResponsibilities => await CalculateAdministrativeResponsibilities(type, baseValue, teacherId, periodId),
            NonTeachingLoadType.SyndicalAndPoliticalResposabilities => await CalculateSyndicalAndPoliticalResponsibilities(type, baseValue, teacherId, periodId, teacher),
            NonTeachingLoadType.ProcessResponsibilities => await CalculateProcessResponsibilities(type, baseValue, teacherId, periodId, teacher),
            _ => throw new NonTeachingLoadUnsettableException(),
        };
    }

    private async Task<bool> CalculateProcessResponsibilities(NonTeachingLoadType type, string baseValue, Guid teacherId, Guid periodId, TeacherModel teacher)
    {
        if (!Enum.TryParse(baseValue, out TeacherProcessResponsabilities artype))
        {
            throw new ArgumentException($"The base value supplied for {artype} load type is invalid.", nameof(baseValue));
        }

        double? calculationValue = _calculationOptions[$"{nameof(TeacherProcessResponsabilities)}.{artype}"];
        if (calculationValue is null)
        {
            return true;
        }

        double loadValue = calculationValue.Value * await _periodsManager.GetPeriodMonthsCountAsync(periodId);

        if (teacher.ContractType == TeacherContractType.PartTime)
        {
            loadValue = Math.Round(loadValue * teacher.SpecificTimeFund / _calculationOptions.MonthTimeFund);
        }

        NonTeachingLoadModel? existingLoad = await GetTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
        if (existingLoad is not null)
        {
            existingLoad.BaseValue = JsonConvert.SerializeObject(artype);
            existingLoad.Load = Math.Round(loadValue, 2);
            existingLoad.Description = $"{artype.GetEnumDisplayNameValue()} - {artype.GetEnumDisplayDescriptionValue()}";
            _ = _context.NonTeachingLoad.Update(existingLoad);
        }
        else
        {
            NonTeachingLoadModel newLoad = new()
            {
                BaseValue = JsonConvert.SerializeObject(artype),
                Load = Math.Round(loadValue, 2),
                Description = $"{artype.GetEnumDisplayNameValue()} - {artype.GetEnumDisplayDescriptionValue()}",
                Type = type,
                TeacherId = teacherId,
                PeriodId = periodId
            };
            _ = _context.NonTeachingLoad.Add(newLoad);
        }

        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> CalculateSyndicalAndPoliticalResponsibilities(NonTeachingLoadType type, string baseValue, Guid teacherId, Guid periodId, TeacherModel teacher)
    {
        if (!Enum.TryParse(baseValue, out TeacherSyndicalAndPoliticalResposabilities sprtype))
        {
            throw new ArgumentException($"The base value supplied for {sprtype} load type is invalid.", nameof(baseValue));
        }

        double? calculationValue = _calculationOptions[$"{nameof(TeacherSyndicalAndPoliticalResposabilities)}.{sprtype}"];
        if (calculationValue is null)
        {
            return false;
        }

        double loadValue = calculationValue.Value * await _periodsManager.GetPeriodMonthsCountAsync(periodId);

        if (teacher.ContractType == TeacherContractType.PartTime)
        {
            loadValue = Math.Round(loadValue * teacher.SpecificTimeFund / _calculationOptions.MonthTimeFund);
        }

        NonTeachingLoadModel? existingLoad = await GetTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
        if (existingLoad is not null)
        {
            existingLoad.BaseValue = JsonConvert.SerializeObject(sprtype);
            existingLoad.Load = Math.Round(loadValue, 2);
            existingLoad.Description = $"{sprtype.GetEnumDisplayNameValue()} - {sprtype.GetEnumDisplayDescriptionValue()}";
            _ = _context.NonTeachingLoad.Update(existingLoad);
        }
        else
        {
            NonTeachingLoadModel newLoad = new()
            {
                BaseValue = JsonConvert.SerializeObject(sprtype),
                Load = Math.Round(loadValue, 2),
                Description = $"{sprtype.GetEnumDisplayNameValue()} - {sprtype.GetEnumDisplayDescriptionValue()}",
                Type = type,
                TeacherId = teacherId,
                PeriodId = periodId
            };
            _ = _context.NonTeachingLoad.Add(newLoad);
        }

        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> CalculateAdministrativeResponsibilities(NonTeachingLoadType type, string baseValue, Guid teacherId, Guid periodId)
    {
        if (!Enum.TryParse(baseValue, out TeacherAdministrativeResponsibilities frtype))
        {
            throw new ArgumentException($"The base value supplied for {frtype} load type is invalid.", nameof(baseValue));
        }

        double? calculationValue = _calculationOptions[$"{nameof(TeacherAdministrativeResponsibilities)}.{frtype}"];
        if (calculationValue is null)
        {
            return false;
        }

        double loadValue = calculationValue.Value * await _periodsManager.GetPeriodMonthsCountAsync(periodId);

        NonTeachingLoadModel? existingLoad = await GetTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
        if (existingLoad is not null)
        {
            existingLoad.BaseValue = JsonConvert.SerializeObject(frtype);
            existingLoad.Load = Math.Round(loadValue, 2);
            existingLoad.Description = $"{frtype.GetEnumDisplayNameValue()} - {frtype.GetEnumDisplayDescriptionValue()}";
            _ = _context.NonTeachingLoad.Update(existingLoad);
        }
        else
        {
            NonTeachingLoadModel newLoad = new()
            {
                BaseValue = JsonConvert.SerializeObject(frtype),
                Load = Math.Round(loadValue, 2),
                Description = $"{frtype.GetEnumDisplayNameValue()} - {frtype.GetEnumDisplayDescriptionValue()}",
                Type = type,
                TeacherId = teacherId,
                PeriodId = periodId
            };
            _ = _context.NonTeachingLoad.Add(newLoad);
        }

        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> CalculateEducationalWork(NonTeachingLoadType type, string baseValue, Guid teacherId, Guid periodId, TeacherModel teacher)
    {
        if (!Enum.TryParse(baseValue, out EducationalWorkType ewtype))
        {
            throw new ArgumentException($"The base mValue supplied for {ewtype} load type is invalid.", nameof(baseValue));
        }

        double calculationValue = _calculationOptions[$"{nameof(EducationalWorkType)}.{ewtype}"] ?? throw new ConfigurationException();
        double loadValue = calculationValue * await _periodsManager.GetPeriodMonthsCountAsync(periodId);

        if (teacher.ContractType == TeacherContractType.PartTime)
        {
            loadValue = Math.Round(loadValue * teacher.SpecificTimeFund / _calculationOptions.MonthTimeFund);
        }

        NonTeachingLoadModel? existingLoad = await GetTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
        if (existingLoad is not null)
        {
            existingLoad.BaseValue = JsonConvert.SerializeObject(ewtype);
            existingLoad.Load = Math.Round(loadValue, 2);
            existingLoad.Description = $"{ewtype.GetEnumDisplayNameValue()} - {ewtype.GetEnumDisplayDescriptionValue()}";
            _ = _context.NonTeachingLoad.Update(existingLoad);
        }
        else
        {
            NonTeachingLoadModel newEWLoad = new()
            {
                BaseValue = JsonConvert.SerializeObject(ewtype),
                Load = Math.Round(loadValue, 2),
                Description = $"{ewtype.GetEnumDisplayNameValue()} - {ewtype.GetEnumDisplayDescriptionValue()}",
                Type = type,
                TeacherId = teacherId,
                PeriodId = periodId
            };
            _ = _context.NonTeachingLoad.Add(newEWLoad);
        }

        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> CalculateUniversitaryExtensionActions(NonTeachingLoadType type, string baseValue, Guid teacherId, Guid periodId, TeacherModel teacher)
    {
        if (!Enum.TryParse(baseValue, out UniversityExtensionActionsOptions ueoption))
        {
            throw new ArgumentException($"The base mValue supplied for {type} load type is invalid.", nameof(baseValue));
        }

        double calculationValue = _calculationOptions[$"{nameof(UniversityExtensionActionsOptions)}.{ueoption}"] ?? throw new ConfigurationException();
        double loadValue = calculationValue * await _periodsManager.GetPeriodMonthsCountAsync(periodId);

        if (teacher.ContractType == TeacherContractType.PartTime)
        {
            loadValue = Math.Round(loadValue * teacher.SpecificTimeFund / _calculationOptions.MonthTimeFund);
        }

        NonTeachingLoadModel? existingUELoad = await GetTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
        if (existingUELoad is not null)
        {
            existingUELoad.BaseValue = JsonConvert.SerializeObject(ueoption);
            existingUELoad.Load = Math.Round(loadValue, 2);
            existingUELoad.Description = $"{ueoption.GetEnumDisplayNameValue()} - {ueoption.GetEnumDisplayDescriptionValue()}";
            _ = _context.NonTeachingLoad.Update(existingUELoad);
        }
        else
        {
            NonTeachingLoadModel newUELoad = new()
            {
                BaseValue = JsonConvert.SerializeObject(ueoption),
                Load = Math.Round(loadValue, 2),
                Description = $"{ueoption.GetEnumDisplayNameValue()} - {ueoption.GetEnumDisplayDescriptionValue()}",
                Type = type,
                TeacherId = teacherId,
                PeriodId = periodId
            };
            _ = _context.NonTeachingLoad.Add(newUELoad);
        }

        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> CalculateParticipationInProjects(NonTeachingLoadType type, string baseValue, Guid teacherId, Guid periodId, TeacherModel teacher)
    {
        if (!Enum.TryParse(baseValue, out ParticipationInProjectsOptions ppOption))
        {
            throw new ArgumentException($"The base mValue supplied for {type} load type is invalid.", nameof(baseValue));
        }

        double? calculationValue = _calculationOptions[$"{nameof(ParticipationInProjectsOptions)}.{ppOption}"] ?? throw new ConfigurationException();
        double loadValue = calculationValue.Value * await _periodsManager.GetPeriodMonthsCountAsync(periodId);
        if (teacher.ContractType == TeacherContractType.PartTime)
        {
            loadValue = Math.Round(loadValue * teacher.SpecificTimeFund / _calculationOptions.MonthTimeFund, 2);
        }

        NonTeachingLoadModel? existingUELoad = await GetTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
        if (existingUELoad is not null)
        {
            existingUELoad.BaseValue = JsonConvert.SerializeObject(ppOption);
            existingUELoad.Load = Math.Round(loadValue, 2);
            existingUELoad.Description = $"{ppOption.GetEnumDisplayNameValue()} - {ppOption.GetEnumDisplayDescriptionValue()}";
            _ = _context.NonTeachingLoad.Update(existingUELoad);
        }
        else
        {
            NonTeachingLoadModel newUELoad = new()
            {
                BaseValue = JsonConvert.SerializeObject(ppOption),
                Load = Math.Round(loadValue, 2),
                Description = $"{ppOption.GetEnumDisplayNameValue()} - {ppOption.GetEnumDisplayDescriptionValue()}",
                Type = type,
                TeacherId = teacherId,
                PeriodId = periodId
            };
            _ = _context.NonTeachingLoad.Add(newUELoad);
        }

        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> CalculateGraduateTutoring(NonTeachingLoadType type, string baseValue, Guid teacherId, Guid periodId)
    {
        GraduateTutoringModel gtModel = JsonConvert.DeserializeObject<GraduateTutoringModel>(baseValue) ?? throw new ArgumentException($"The base mValue supplied for {type} load type is invalid.", nameof(baseValue));
        double? dmCalculationBase = _calculationOptions[$"{nameof(GraduateTutoringModel)}.{nameof(gtModel.DiplomaOrMastersDegreeDiplomants)}"];
        double? dCalculationBase = _calculationOptions[$"{nameof(GraduateTutoringModel)}.{nameof(gtModel.DoctorateDiplomants)}"];
        if (dmCalculationBase is null || dCalculationBase is null)
        {
            return false;
        }

        double monthCount = await _periodsManager.GetPeriodMonthsCountAsync(periodId);
        double loadValue = (gtModel.DiplomaOrMastersDegreeDiplomants * dmCalculationBase.Value * monthCount) + (gtModel.DoctorateDiplomants * dmCalculationBase.Value * monthCount);
        NonTeachingLoadModel? existingLoad = await GetTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
        if (existingLoad is not null)
        {
            existingLoad.BaseValue = baseValue;
            existingLoad.Load = Math.Round(loadValue, 2);
            existingLoad.Description = $"Tutorados estimados: {gtModel.DiplomaOrMastersDegreeDiplomants} de diplmado y/o maestría, y {gtModel.DoctorateDiplomants} de doctorado.";
            _ = _context.NonTeachingLoad.Update(existingLoad);
        }
        else
        {
            NonTeachingLoadModel newUELoad = new()
            {
                BaseValue = baseValue,
                Load = Math.Round(loadValue, 2),
                Description = $"Tutorados estimados: {gtModel.DiplomaOrMastersDegreeDiplomants} de diplmado y/o maestría, y {gtModel.DoctorateDiplomants} de doctorado.",
                Type = type,
                TeacherId = teacherId,
                PeriodId = periodId
            };
            _ = _context.NonTeachingLoad.Add(newUELoad);
        }

        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> CalculateUndergraduateTutoring(NonTeachingLoadType type, string baseValue, Guid teacherId, Guid periodId)
    {
        UndergraduateTutoringModel utModel = JsonConvert.DeserializeObject<UndergraduateTutoringModel>(baseValue) ?? throw new ArgumentException($"The base mValue supplied for {type} load type is invalid.", nameof(baseValue));
        double? ipCalculationBase = _calculationOptions[$"{nameof(UndergraduateTutoringModel)}.{nameof(utModel.IntegrativeProjectDiplomants)}"];
        double? tCalculationBase = _calculationOptions[$"{nameof(UndergraduateTutoringModel)}.{nameof(utModel.ThesisDiplomants)}"];
        if (ipCalculationBase is null || tCalculationBase is null)
        {
            return false;
        }

        double monthCount = await _periodsManager.GetPeriodMonthsCountAsync(periodId);
        double loadValue = (utModel.IntegrativeProjectDiplomants * ipCalculationBase.Value) + (utModel.ThesisDiplomants * tCalculationBase.Value * monthCount);
        NonTeachingLoadModel? existingLoad = await GetTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
        if (existingLoad is not null)
        {
            existingLoad.BaseValue = baseValue;
            existingLoad.Load = Math.Round(loadValue, 2);
            existingLoad.Description = $"Diplomantes estimados: {utModel.IntegrativeProjectDiplomants} de proyecto integrador y {utModel.ThesisDiplomants} de tesis";
            _ = _context.NonTeachingLoad.Update(existingLoad);
        }
        else
        {
            NonTeachingLoadModel newUELoad = new()
            {
                BaseValue = baseValue,
                Load = Math.Round(loadValue, 2),
                Description = $"Diplomantes estimados: {utModel.IntegrativeProjectDiplomants} de proyecto integrador y {utModel.ThesisDiplomants} de tesis",
                Type = type,
                TeacherId = teacherId,
                PeriodId = periodId
            };
            _ = _context.NonTeachingLoad.Add(newUELoad);
        }

        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> CalculateCoursesReceivedAndImprovement(NonTeachingLoadType type, string baseValue, Guid teacherId, Guid periodId, TeacherModel teacher)
    {
        if (!Enum.TryParse(baseValue, out CoursesReceivedAndImprovementOptions option))
        {
            throw new ArgumentException($"The base mValue supplied for {type} load type is invalid.", nameof(baseValue));
        }

        double calculationValue = _calculationOptions[$"{nameof(CoursesReceivedAndImprovementOptions)}.{option}"] ?? throw new ConfigurationException();
        double loadValue = calculationValue * await _periodsManager.GetPeriodMonthsCountAsync(periodId);

        if (teacher.ContractType == TeacherContractType.PartTime)
        {
            loadValue = Math.Round(loadValue * teacher.SpecificTimeFund / _calculationOptions.MonthTimeFund, 2);
        }

        NonTeachingLoadModel? existingLoad = await GetTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
        if (existingLoad is not null)
        {
            existingLoad.BaseValue = JsonConvert.SerializeObject(option);
            existingLoad.Load = Math.Round(loadValue, 2);
            existingLoad.Description = $"{option.GetEnumDisplayNameValue()} - {option.GetEnumDisplayDescriptionValue()}";
            _ = _context.NonTeachingLoad.Update(existingLoad);
            return await _context.SaveChangesAsync() > 0;
        }

        NonTeachingLoadModel newUELoad = new()
        {
            BaseValue = JsonConvert.SerializeObject(option),
            Load = Math.Round(loadValue, 2),
            Description = $"{option.GetEnumDisplayNameValue()} - {option.GetEnumDisplayDescriptionValue()}",
            Type = type,
            TeacherId = teacherId,
            PeriodId = periodId
        };
        _ = _context.NonTeachingLoad.Add(newUELoad);

        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> CalculatePostgraduateThesisCourtAndRevision(NonTeachingLoadType type, string baseValue, Guid teacherId, Guid periodId)
    {
        PostgraduateThesisCourtModel ptcModel = JsonConvert.DeserializeObject<PostgraduateThesisCourtModel>(baseValue) ?? throw new ArgumentException($"The base mValue supplied for {type} load type is invalid.", nameof(baseValue));
        double? ptcDmCalculationBase = _calculationOptions[$"{nameof(PostgraduateThesisCourtModel)}.{nameof(ptcModel.MastersAndDiplomantsThesisCourts)}"];
        double? ptcPhdCalculationBase = _calculationOptions[$"{nameof(PostgraduateThesisCourtModel)}.{nameof(ptcModel.DoctorateThesisCourts)}"];
        if (ptcDmCalculationBase is null || ptcPhdCalculationBase is null)
        {
            throw new ConfigurationException();
        }

        double? loadValue = (ptcModel.MastersAndDiplomantsThesisCourts * ptcDmCalculationBase) + (ptcModel.DoctorateThesisCourts * ptcPhdCalculationBase);
        NonTeachingLoadModel? existingLoad = await GetTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
        if (existingLoad is not null)
        {
            existingLoad.BaseValue = JsonConvert.SerializeObject(ptcModel);
            existingLoad.Load = Math.Round(loadValue.Value, 2);
            existingLoad.Description = $"Tribunales estimados: {ptcModel.MastersAndDiplomantsThesisCourts} de maestría, postgrado y/o diplomado, {ptcModel.DoctorateThesisCourts} de doctorado";
            _ = _context.NonTeachingLoad.Update(existingLoad);
            return await _context.SaveChangesAsync() > 0;
        }

        NonTeachingLoadModel newPTCLoad = new()
        {
            BaseValue = baseValue,
            Load = Math.Round(loadValue.Value, 2),
            Description = $"Tribunales estimados: {ptcModel.MastersAndDiplomantsThesisCourts} de maestría, postgrado y/o diplomado, {ptcModel.DoctorateThesisCourts} de doctorado",
            Type = type,
            TeacherId = teacherId,
            PeriodId = periodId
        };
        _ = _context.NonTeachingLoad.Add(newPTCLoad);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<double> GetTeacherTimeFund(Guid teacherId, Guid periodId)
    {
        if (periodId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(periodId));
        }

        if (!await _periodsManager.ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }

        if (teacherId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(teacherId));
        }

        if (!await _teachersManager.ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }

        var teacherInfo = await _context.Teachers.Where(t => t.Id == teacherId).Select(t => new { t.ContractType, t.SpecificTimeFund }).FirstAsync();
        var periodInfo = await _context.Periods.Where(p => p.Id == periodId).Select(p => new { p.MonthsCount, p.TimeFund }).FirstAsync();
        double timeFund = teacherInfo.ContractType is TeacherContractType.FullTime or TeacherContractType.Collaborator ? periodInfo.TimeFund : periodInfo.MonthsCount * teacherInfo.SpecificTimeFund;
        return timeFund;
    }
}
