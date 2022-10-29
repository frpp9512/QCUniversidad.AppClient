﻿using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.Api.Shared.Extensions;

namespace QCUniversidad.Api.Services;

#region Exceptions

public class FacultyNotFoundException : Exception { }
public class DepartmentNotFoundException : Exception { }
public class CareerNotFoundException : Exception { }
public class DisciplineNotFoundException : Exception { }
public class TeacherNotFoundException : Exception { }
public class SubjectNotFoundException : Exception { }
public class CurriculumNotFoundException : Exception { }
public class NotCurrentSchoolYearDefined : Exception { }
public class SchoolYearNotFoundException : Exception { }
public class CourseNotFoundException : Exception { }
public class PeriodNotFoundException : Exception { }
public class TeachingPlanNotFoundException : Exception { }
public class TeachingPlanItemNotFoundException : Exception { }
public class PlanItemFullyCoveredException : Exception { }
public class LoadItemNotFoundException : Exception { }
public class PeriodSubjectNotFoundException : Exception { }

public class DatabaseOperationException : Exception { }

#endregion

public class DataManager : IDataManager
{
    private readonly QCUniversidadContext _context;
    private readonly ICoefficientCalculator<TeachingPlanItemModel> _planItemCalculator;
    private readonly ICoefficientCalculator<PeriodModel> _periodCalculator;
    private readonly CalculationOptions _calculationOptions;

    public DataManager(QCUniversidadContext context, ICoefficientCalculator<TeachingPlanItemModel> planItemCalculator, ICoefficientCalculator<PeriodModel> periodCalculator, IOptions<CalculationOptions> options)
    {
        _context = context;
        _planItemCalculator = planItemCalculator;
        _periodCalculator = periodCalculator;
        _calculationOptions = options.Value;
    }

    #region Faculties

    public async Task<FacultyModel> GetFacultyAsync(Guid id)
    {
        var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Id.Equals(id));
        return faculty is null ? throw new FacultyNotFoundException() : faculty;
    }

    public async Task<IList<FacultyModel>> GetFacultiesAsync(int from = 0, int to = 0)
    {
        var faculties =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.Faculties.Skip(from).Take(to).ToListAsync()
            : await _context.Faculties.ToListAsync();
        return faculties;
    }

    public async Task<int> GetFacultiesTotalAsync()
    {
        var total = await _context.Faculties.CountAsync();
        return total;
    }

    public async Task<bool> ExistFacultyAsync(Guid id)
    {
        var result = await _context.Faculties.AnyAsync(f => f.Id == id);
        return result;
    }

    public async Task<bool> CreateFacultyAsync(FacultyModel faculty)
    {
        if (faculty is not null)
        {
            _ = await _context.AddAsync(faculty);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(faculty));
    }

    public async Task<bool> UpdateFacultyAsync(FacultyModel faculty)
    {
        if (faculty is not null)
        {
            _ = _context.Update(faculty);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(faculty));
    }

    public async Task<int> GetFacultyDepartmentCountAsync(Guid facultyId)
    {
        var count = await _context.Departments.CountAsync(d => d.FacultyId == facultyId);
        return count;
    }

    public async Task<int> GetFacultyCareerCountAsync(Guid facultyId)
    {
        var count = await _context.Careers.CountAsync(c => c.FacultyId.Equals(facultyId));
        return count;
    }

    public async Task<bool> DeleteFacultyAsync(Guid facultyId)
    {
        try
        {
            var faculty = await GetFacultyAsync(facultyId);
            _ = _context.Remove(faculty);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (FacultyNotFoundException)
        {
            throw;
        }
    }

    #endregion

    #region Departments

    public async Task<IList<DepartmentModel>> GetDepartmentsAsync(int from, int to)
    {
        var deparments = (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
                         ? await _context.Departments.Skip(from).Take(to).Include(d => d.Faculty).ToListAsync()
                         : await _context.Departments.Include(d => d.Faculty).ToListAsync();
        return deparments;
    }

    public async Task<int> GetDepartmentDisciplinesCount(Guid departmentId)
    {
        var count = await _context.Disciplines.CountAsync(d => d.DepartmentId == departmentId);
        return count;
    }

    public async Task<bool> ExistDepartmentAsync(Guid id)
    {
        var result = await _context.Departments.AnyAsync(f => f.Id == id);
        return result;
    }

    public async Task<IList<DepartmentModel>> GetDepartmentsAsync(Guid facultyId)
    {
        var deparments = await _context.Departments.Where(d => d.FacultyId == facultyId).ToListAsync();
        return deparments;
    }

    public async Task<int> GetDepartmentsCountAsync()
    {
        var count = await _context.Departments.CountAsync();
        return count;
    }

    public async Task<int> GetDepartmentsCountAsync(Guid facultyId)
    {
        var count = await _context.Departments.CountAsync(d => d.FacultyId == facultyId);
        return count;
    }

    public async Task<DepartmentModel> GetDepartmentAsync(Guid departmentId)
    {
        var department = await _context.Departments.Where(d => d.Id == departmentId)
                                                   .Include(d => d.Faculty)
                                                   .FirstOrDefaultAsync();
        return department ?? throw new DepartmentNotFoundException();
    }

    public async Task<int> GetDeparmentTeachersCountAsync(Guid departmentId)
    {
        if (!await ExistDepartmentAsync(departmentId))
        {
            throw new DepartmentNotFoundException();
        }
        var count = await _context.Teachers.CountAsync(t => t.DepartmentId == departmentId);
        return count;
    }

    public async Task<bool> CreateDepartmentAsync(DepartmentModel department)
    {
        if (department is null)
        {
            throw new ArgumentNullException(nameof(department));
        }
        _ = await _context.Departments.AddAsync(department);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateDeparmentAsync(DepartmentModel department)
    {
        if (department is null)
        {
            throw new ArgumentNullException(nameof(department));
        }
        _ = _context.Departments.Update(department);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteDeparmentAsync(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(departmentId));
        }
        var department = await GetDepartmentAsync(departmentId);
        _ = _context.Departments.Remove(department);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<double> GetTotalLoadInPeriodAsync(Guid periodId)
    {
        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }
        var query = from planItem in _context.TeachingPlanItems
                    where planItem.PeriodId == periodId
                    select planItem.TotalHoursPlanned;

        var result = await query.SumAsync();

        return result;
    }

    public async Task<double> GetDepartmentTotalLoadInPeriodAsync(Guid periodId, Guid departmentId)
    {
        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }
        var query = from planItem in _context.TeachingPlanItems
                    where planItem.PeriodId == periodId
                    join subject in _context.Subjects
                    on planItem.SubjectId equals subject.Id
                    join discipline in _context.Disciplines
                    on subject.DisciplineId equals discipline.Id
                    join department in _context.Departments
                    on discipline.DepartmentId equals department.Id
                    where discipline.DepartmentId == department.Id
                    select planItem.TotalHoursPlanned;
        var listResult = await query.ToListAsync();

        var result = listResult.Sum();

        return result;
    }

    public async Task<double> GetTotalLoadCoveredInPeriodAsync(Guid periodId)
    {
        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }
        var query = from loadItem in _context.LoadItems
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

        var result = await query.SumAsync();

        return result;
    }

    public async Task<double> GetDepartmentTotalLoadCoveredInPeriodAsync(Guid periodId, Guid departmentId)
    {
        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }
        var query = from loadItem in _context.LoadItems
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

        var result = await query.SumAsync();

        return result;
    }

    public async Task<double> GetDepartmentTotalTimeFund(Guid departmentId, Guid periodId)
    {
        if (!await ExistDepartmentAsync(departmentId))
        {
            throw new DepartmentNotFoundException();
        }
        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }
        var periodTimeFund = await GetPeriodTimeFund(periodId);
        var teachersCount = await GetDeparmentTeachersCountAsync(departmentId);
        return periodTimeFund * teachersCount;
    }

    public async Task<double> CalculateRAPAsync(Guid departmentId)
    {
        var departmentDisciplines = from discipline in _context.Disciplines
                                    where discipline.DepartmentId == departmentId
                                    select discipline;

        var departmentSubjects = from subject in _context.Subjects
                                 join discipline in _context.Disciplines
                                 on subject.DisciplineId equals discipline.Id
                                 select subject;

        var planItems = from planItem in _context.TeachingPlanItems
                        join subject in departmentSubjects
                        on planItem.SubjectId equals subject.Id
                        select planItem;

        var courses = from course in _context.Courses
                      join planItem in planItems
                      on course.Id equals planItem.CourseId
                      select course;

        courses = courses.Distinct().Include(c => c.Career);
        var totalEnrolment = await courses.SumAsync(c => c.Career.PostgraduateCourse ? c.Enrolment / 3 : c.Enrolment);

        var teachers = from teacher in _context.Teachers
                       where teacher.DepartmentId == departmentId && teacher.Active
                       select teacher;

        return totalEnrolment / await teachers.CountAsync();
    }

    #endregion

    #region Careers

    public async Task<bool> ExistsCareerAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var result = await _context.Careers.AnyAsync(c => c.Id == id);
            return result;
        }
        throw new ArgumentException(nameof(id));
    }

    public async Task<bool> CreateCareerAsync(CareerModel career)
    {
        if (career is not null)
        {
            _ = await _context.Careers.AddAsync(career);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(career));
    }

    public async Task<IList<CareerModel>> GetCareersAsync(int from = 0, int to = 0)
    {
        var result = (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
                     ? await _context.Careers.Skip(from).Take(to).Include(c => c.Faculty).ToListAsync()
                     : await _context.Careers.Include(c => c.Faculty).ToListAsync();
        return result;
    }

    public async Task<IList<CareerModel>> GetCareersAsync(Guid facultyId)
    {
        if (facultyId != Guid.Empty)
        {
            var result = await _context.Careers.Where(c => c.FacultyId == facultyId).Include(c => c.Faculty).ToListAsync();
            return result;
        }
        throw new ArgumentNullException(nameof(facultyId));
    }

    public async Task<CareerModel> GetCareerAsync(Guid careerId)
    {
        if (careerId != Guid.Empty)
        {
            var result = await _context.Careers.Where(c => c.Id == careerId).Include(c => c.Faculty).FirstOrDefaultAsync();
            return result ?? throw new CareerNotFoundException();
        }
        throw new ArgumentNullException(nameof(careerId));
    }

    public async Task<int> GetCareersCountAsync()
    {
        var result = await _context.Careers.CountAsync();
        return result;
    }

    public async Task<bool> UpdateCareerAsync(CareerModel career)
    {
        if (career is not null)
        {
            _ = _context.Careers.Update(career);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var query = from planItem in _context.TeachingPlanItems
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
                var career = await GetCareerAsync(careerId);
                _ = _context.Careers.Remove(career);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (CareerNotFoundException)
            {
                throw;
            }
        }
        throw new ArgumentNullException(nameof(careerId));
    }

    #endregion

    #region Disciplines

    public async Task<bool> CreateDisciplineAsync(DisciplineModel discipline)
    {
        if (discipline is not null)
        {
            _ = await _context.Disciplines.AddAsync(discipline);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(discipline));
    }

    public async Task<bool> ExistsDisciplineAsync(Guid id)
    {
        var result = await _context.Disciplines.AnyAsync(d => d.Id == id);
        return result;
    }

    public async Task<IList<DisciplineModel>> GetDisciplinesAsync(int from, int to)
    {
        var result =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.Disciplines.Skip(from).Take(to).Include(d => d.Department).ToListAsync()
            : await _context.Disciplines.Include(d => d.Department).ToListAsync();
        return result;
    }

    public async Task<int> GetDisciplinesCountAsync() => await _context.Disciplines.CountAsync();

    public async Task<int> GetDisciplineSubjectsCountAsync(Guid disciplineId)
    {
        var result = await _context.Subjects.CountAsync(s => s.DisciplineId == disciplineId);
        return result;
    }
    public async Task<int> GetDisciplineTeachersCountAsync(Guid disciplineId)
    {
        var result = await _context.TeachersDisciplines.CountAsync(td => td.DisciplineId == disciplineId);
        return result;
    }

    public async Task<DisciplineModel> GetDisciplineAsync(Guid disciplineId)
    {
        if (disciplineId != Guid.Empty)
        {
            var result = await _context.Disciplines.Where(d => d.Id == disciplineId)
                                                   .Include(d => d.Department)
                                                   .FirstOrDefaultAsync();
            return result ?? throw new DisciplineNotFoundException();
        }
        throw new ArgumentNullException(nameof(disciplineId));
    }

    public async Task<bool> UpdateDisciplineAsync(DisciplineModel discipline)
    {
        if (discipline is not null)
        {
            _ = _context.Disciplines.Update(discipline);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(discipline));
    }

    public async Task<bool> DeleteDisciplineAsync(Guid disciplineId)
    {
        if (disciplineId != Guid.Empty)
        {
            try
            {
                var discipline = await GetDisciplineAsync(disciplineId);
                _ = _context.Disciplines.Remove(discipline);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (DisciplineNotFoundException)
            {
                throw;
            }
        }
        throw new ArgumentNullException(nameof(disciplineId));
    }

    #endregion

    #region Teachers

    public async Task<bool> CreateTeacherAsync(TeacherModel teacher)
    {
        if (teacher is not null)
        {
            _ = await _context.Teachers.AddAsync(teacher);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(teacher));
    }

    public async Task<bool> ExistsTeacherAsync(Guid id)
    {
        var result = await _context.Teachers.AnyAsync(t => t.Id == id);
        return result;
    }

    public async Task<int> GetTeachersCountAsync() => await _context.Teachers.CountAsync();

    public async Task<int> GetTeacherDisciplinesCountAsync(Guid id)
    {
        var result = await _context.TeachersDisciplines.CountAsync(td => td.TeacherId == id);
        return result;
    }

    public async Task<IList<TeacherModel>> GetTeachersAsync(int from, int to)
    {
        var result =
            from != 0 && from == to && from >= 0 && to >= from && !(from == 0 && from == to)
            ? await _context.Teachers.Where(t => t.Active).Skip(from).Take(to).Include(d => d.Department).Include(d => d.TeacherDisciplines).ThenInclude(td => td.Discipline).ToListAsync()
            : await _context.Teachers.Where(t => t.Active).Include(d => d.Department).Include(d => d.TeacherDisciplines).ThenInclude(td => td.Discipline).ToListAsync();
        return result;
    }

    public async Task<TeacherModel> GetTeacherAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var result = await _context.Teachers.Where(t => t.Id == id)
                                                .Include(d => d.Department)
                                                .Include(d => d.TeacherDisciplines).ThenInclude(td => td.Discipline)
                                                .FirstOrDefaultAsync();
            return result ?? throw new TeacherNotFoundException();
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<bool> UpdateTeacherAsync(TeacherModel teacher)
    {
        if (teacher is not null)
        {
            await _context.TeachersDisciplines.Where(td => td.TeacherId == teacher.Id)
                                              .ForEachAsync(td => _context.Remove(td));
            await _context.TeachersDisciplines.AddRangeAsync(teacher.TeacherDisciplines);
            _ = _context.Teachers.Update(teacher);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(teacher));
    }

    public async Task<bool> DeleteTeacherAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            try
            {
                var teacher = await GetTeacherAsync(id);
                if (!await TeacherHaveLoad(id))
                {
                    _ = _context.Teachers.Remove(teacher);
                }
                else
                {
                    teacher.Active = false;
                    _ = _context.Teachers.Update(teacher);
                }
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (TeacherNotFoundException)
            {
                throw;
            }
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<IList<TeacherModel>> GetTeachersOfDepartmentAsync(Guid departmentId)
    {
        try
        {
            if (!await ExistDepartmentAsync(departmentId))
            {
                throw new ArgumentNullException();
            }
            var query = from t in _context.Teachers
                        where t.Active && t.DepartmentId == departmentId
                        select t;
            return await query.Include(t => t.TeacherDisciplines).ThenInclude(td => td.Discipline).ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IList<LoadItemModel>> GetTeacherLoadItemsInPeriodAsync(Guid teacherId, Guid periodId)
    {
        if (!await ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }
        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }
        var query = from loadItem in _context.LoadItems
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
        if (!await ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }
        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }
        var items = await _context.NonTeachingLoad.Where(l => l.TeacherId == teacherId && l.PeriodId == periodId).ToListAsync();
        var types = Enum.GetValues<NonTeachingLoadType>();
        foreach (var type in types.Where(t => t.GetEnumDisplayAutogenerateValue()))
        {
            if (!items.Any(i => i.Type == type))
            {
                try
                {
                    var item = await RecalculateTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
                    items.Add(item);
                }
                catch
                { }
            }
        }
        return items;
    }

    public async Task<NonTeachingLoadModel> GetTeacherNonTeachingLoadItemInPeriodAsync(NonTeachingLoadType type, Guid teacherId, Guid periodId)
    {
        if (!await ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }
        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }
        var itemQuery = from ntl in _context.NonTeachingLoad
                        where ntl.TeacherId == teacherId && ntl.PeriodId == periodId && ntl.Type == type
                        select ntl;
        return await itemQuery.FirstOrDefaultAsync();
    }

    public async Task<NonTeachingLoadModel> RecalculateTeacherNonTeachingLoadItemInPeriodAsync(NonTeachingLoadType type, Guid teacherId, Guid periodId)
    {
        if (type.GetEnumDisplayAutogenerateValue())
        {
            var loadItem = await GetTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
            var calculated = await CalculateNonTeachingLoadOfTypeAsync(type, teacherId, periodId);
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
            var result = await _context.SaveChangesAsync();
            return result > 0 ? (loadItem is null ? calculated : loadItem) : throw new DatabaseOperationException();
        }
        throw new ArgumentException("The non-teaching load type should be auto-generated.", nameof(type));
    }

    private async Task<NonTeachingLoadModel> CalculateNonTeachingLoadOfTypeAsync(NonTeachingLoadType type, Guid teacherId, Guid periodId)
    {
        switch (type)
        {
            case NonTeachingLoadType.Consultation:
                var cQuery = from loadItem in _context.LoadItems
                             join planItem in _context.TeachingPlanItems
                             on loadItem.PlanningItemId equals planItem.Id
                             where loadItem.TeacherId == teacherId
                                   && planItem.PeriodId == periodId
                                   && !planItem.FromPostgraduateCourse
                             select planItem.SubjectId;
                var cValue = cQuery.Distinct().Count();
                var cItem = new NonTeachingLoadModel
                {
                    Type = NonTeachingLoadType.Consultation,
                    Description = NonTeachingLoadType.Consultation.GetEnumDisplayDescriptionValue(),
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(cValue),
                    Load = Math.Round(cValue * _calculationOptions.ConsultationCoefficient, 2)
                };
                return cItem;
            case NonTeachingLoadType.ClassPreparation:
                var cpQuery = from loadItem in _context.LoadItems
                              join planItem in _context.TeachingPlanItems
                              on loadItem.PlanningItemId equals planItem.Id
                              where loadItem.TeacherId == teacherId
                                    && planItem.PeriodId == periodId
                              select new { hoursCovered = loadItem.HoursCovered, type = planItem.Type };
                var calculationModel = new ClassPreparationCalculationModel
                {
                    MainClassesValue = await cpQuery.SumAsync(value => value.type == TeachingActivityType.Conference || value.type == TeachingActivityType.PostgraduateClass || value.type == TeachingActivityType.MeetingClass ? value.hoursCovered : 0),
                    SecondaryClassesValue = await cpQuery.SumAsync(value => value.type != TeachingActivityType.Conference && value.type != TeachingActivityType.PostgraduateClass && value.type != TeachingActivityType.MeetingClass ? value.hoursCovered : 0)
                };
                var cpItem = new NonTeachingLoadModel
                {
                    Type = NonTeachingLoadType.ClassPreparation,
                    Description = NonTeachingLoadType.ClassPreparation.GetEnumDisplayDescriptionValue(),
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(calculationModel),
                    Load = Math.Round((calculationModel.MainClassesValue * _calculationOptions.ClassPreparationPrimaryCoefficient) + (calculationModel.SecondaryClassesValue * _calculationOptions.ClassPreparationSecondaryCoefficient), 2)
                };
                return cpItem;
            case NonTeachingLoadType.CoursesReceivedAndImprovement:
                return null;
            case NonTeachingLoadType.Meetings:
                var mQuery = from period in _context.Periods
                             where period.Id == periodId
                             select period.MonthsCount;
                var mValue = await mQuery.FirstAsync();

                var mItem = new NonTeachingLoadModel
                {
                    Type = NonTeachingLoadType.Meetings,
                    Description = NonTeachingLoadType.Meetings.GetEnumDisplayDescriptionValue(),
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(mValue),
                    Load = Math.Round(mValue * _calculationOptions.MeetingsCoefficient, 2)
                };
                return mItem;
            case NonTeachingLoadType.MethodologicalActions:
                var mtQuery = from period in _context.Periods
                              where period.Id == periodId
                              select period.MonthsCount;
                var mtValue = await mtQuery.FirstAsync();

                var mtItem = new NonTeachingLoadModel
                {
                    Type = NonTeachingLoadType.MethodologicalActions,
                    Description = NonTeachingLoadType.MethodologicalActions.GetEnumDisplayDescriptionValue(),
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(mtValue),
                    Load = Math.Round(mtValue * _calculationOptions.MethodologicalActionsCoefficient, 2)
                };
                return mtItem;
            case NonTeachingLoadType.EventsAndPublications:
                var eQuery = from period in _context.Periods
                             where period.Id == periodId
                             select period.MonthsCount;
                var eValue = await eQuery.FirstAsync();

                var eItem = new NonTeachingLoadModel
                {
                    Type = NonTeachingLoadType.EventsAndPublications,
                    Description = NonTeachingLoadType.EventsAndPublications.GetEnumDisplayDescriptionValue(),
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(eValue),
                    Load = Math.Round(eValue * _calculationOptions.EventsAndPublicationsCoefficient, 2)
                };
                return eItem;
            case NonTeachingLoadType.OtherActivities:
                var oaQuery = from period in _context.Periods
                              where period.Id == periodId
                              select period.MonthsCount;
                var oaValue = await oaQuery.FirstAsync();

                var oaItem = new NonTeachingLoadModel
                {
                    Type = NonTeachingLoadType.OtherActivities,
                    Description = NonTeachingLoadType.OtherActivities.GetEnumDisplayDescriptionValue(),
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(oaValue),
                    Load = Math.Round(oaValue * _calculationOptions.OtherActivitiesCoefficient, 2)
                };
                return oaItem;
            case NonTeachingLoadType.ExamGrade:
                var planItemsQuery = from loadItem in _context.LoadItems
                                     join planItem in _context.TeachingPlanItems
                                     on loadItem.PlanningItemId equals planItem.Id
                                     where loadItem.TeacherId == teacherId && planItem.PeriodId == periodId
                                     select new { PlanItemId = planItem.Id, planItem.CourseId, planItem.SubjectId };

                double total = 0;

                foreach (var info in planItemsQuery.Distinct())
                {
                    var teachersCountQuery = from planItem in _context.TeachingPlanItems
                                             join loadItem in _context.LoadItems
                                             on planItem.Id equals loadItem.PlanningItemId
                                             where planItem.Id == info.PlanItemId
                                             select loadItem.TeacherId;

                    var subject = await _context.PeriodSubjects.Where(ps => ps.SubjectId == info.SubjectId).FirstOrDefaultAsync();

                    var teachersCount = await teachersCountQuery.Distinct().CountAsync();
                    var courseEnrolment = await _context.Courses.Where(c => c.Id == info.CourseId).Select(c => c.Enrolment).FirstAsync();
                    var midTermExams = subject.MidtermExamsCount;
                    var finalExams = subject.HaveFinalExam ? 1 : 0;

                    var midTermExamGrade = midTermExams * _calculationOptions.ExamGradeMidTermAverageTime * courseEnrolment;
                    var finalExamGrade = finalExams * _calculationOptions.ExamGradeFinalAverageTime * courseEnrolment;
                    var secondFinalExamGrade = (finalExamGrade * _calculationOptions.SecondExamGradeFinalCoefficient) * _calculationOptions.ExamGradeFinalAverageTime * courseEnrolment;
                    var thirdFinalExamGrade = (finalExamGrade * _calculationOptions.ThirdExamGradeFinalCoefficient) * _calculationOptions.ExamGradeFinalAverageTime * courseEnrolment;

                    var examGrade = (midTermExamGrade + finalExamGrade + secondFinalExamGrade + thirdFinalExamGrade) / teachersCount;

                    total += examGrade;
                }

                var examGradeLoad = new NonTeachingLoadModel
                {
                    Type = NonTeachingLoadType.ExamGrade,
                    Description = NonTeachingLoadType.ExamGrade.GetEnumDisplayDescriptionValue(),
                    TeacherId = teacherId,
                    PeriodId = periodId,
                    BaseValue = JsonConvert.SerializeObject(total),
                    Load = Math.Round(total, 2)
                };
                return examGradeLoad;
            case NonTeachingLoadType.ThesisCourtAndRevision:
                return null;
            case NonTeachingLoadType.UndergraduateTutoring:
                return null;
            case NonTeachingLoadType.GraduateTutoring:
                return null;
            case NonTeachingLoadType.ParticipationInProjects:
                return null;
            case NonTeachingLoadType.UniversityExtensionActions:
                return null;
            case NonTeachingLoadType.OtherFunctions:
                return null;
            default:
                return null;
        }
    }

    public async Task<double> GetTeacherLoadInPeriodAsync(Guid teacherId, Guid periodId)
    {
        if (teacherId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(teacherId));
        }
        if (!await ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }
        if (periodId == Guid.Empty)
        {
            throw new ArgumentException(nameof(periodId));
        }
        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }
        var query = from loadItem in _context.LoadItems
                    join planItem in _context.TeachingPlanItems
                    on loadItem.PlanningItemId equals planItem.Id
                    where loadItem.TeacherId == teacherId && planItem.PeriodId == periodId
                    select loadItem;

        var nonTeachingLoadQuery = from ntl in _context.NonTeachingLoad
                                   where ntl.TeacherId == teacherId && ntl.PeriodId == periodId
                                   select ntl.Load;

        var nonTeachingValue = Math.Round(await _context.NonTeachingLoad.Where(item => item.TeacherId == teacherId && item.PeriodId == periodId).SumAsync(item => item.Load), 2);
        if (nonTeachingValue == 0)
        {
            await RecalculateAutogenerateTeachingLoadItemsAsync(teacherId, periodId);
            nonTeachingValue = await _context.NonTeachingLoad.Where(item => item.TeacherId == teacherId && item.PeriodId == periodId).SumAsync(item => item.Load);
        }

        return await query.SumAsync(i => i.HoursCovered) + nonTeachingValue;
    }

    private async Task RecalculateAutogenerateTeachingLoadItemsAsync(Guid teacherId, Guid periodId)
    {
        var types = Enum.GetValues<NonTeachingLoadType>().Where(type => type.GetEnumDisplayAutogenerateValue());
        foreach (var type in types)
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
        var recalculableTypes = Enum.GetValues<NonTeachingLoadType>().Where(type => type.IsRecalculable());
        foreach (var type in recalculableTypes)
        {
            try
            {
                _ = await RecalculateTeacherNonTeachingLoadItemInPeriodAsync(type, teacherId, periodId);
            }
            catch { }
        }
    }

    private async Task<bool> TeacherHaveLoad(Guid teacherId)
    {
        if (teacherId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(teacherId));
        }
        if (!await ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }
        var query = from teacher in _context.Teachers
                    where teacher.Id == teacherId
                    select teacher;

        return await query.CountAsync() > 0;
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
        //var depTeachersQuery = from teacher in _context.Teachers
        //                       join teacherDiscipline in _context.TeachersDisciplines
        //                       on teacher.Id equals teacherDiscipline.TeacherId
        //                       where (teacher.Active || teacher.DepartmentId == departmentId)
        //                             //|| !disciplineId.HasValue
        //                             //|| teacherDiscipline.DisciplineId == disciplineId
        //                       select teacher;
        var depTeachersQuery = from teacher in _context.Teachers
                               where teacher.DepartmentId == departmentId
                               select teacher;
        depTeachersQuery = depTeachersQuery.Distinct();

        var planItemLoads = from planItem in _context.TeachingPlanItems
                            join loadItem in _context.LoadItems
                            on planItem.Id equals loadItem.PlanningItemId
                            where planItem.Id == planItemId
                            select loadItem;

        var depTeachersInPlanItemQuery = from teacher in _context.Teachers
                                         join loadItem in planItemLoads
                                         on teacher.Id equals loadItem.TeacherId
                                         where teacher.Active
                                         select teacher;

        var finalQuery = depTeachersQuery.Except(depTeachersInPlanItemQuery).Where(t => t.Active);

        finalQuery = finalQuery.Include(t => t.Department);
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
        if (!await ExistsTeacherAsync(teacherId))
        {
            throw new TeacherNotFoundException();
        }
        if (!await ExistsTeachingPlanItemAsync(planItemId))
        {
            throw new TeachingPlanItemNotFoundException();
        }
        if (hours <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hours), "The hours amount should be greater than zero.");
        }
        var planItem = await GetTeachingPlanItemAsync(planItemId);
        var hoursToCover = planItem.TotalHoursPlanned - planItem.LoadItems.Sum(i => i.HoursCovered);
        if (hoursToCover <= 0)
        {
            throw new PlanItemFullyCoveredException();
        }
        var loadItem = new LoadItemModel
        {
            TeacherId = teacherId,
            PlanningItemId = planItemId,
            HoursCovered = hours > hoursToCover ? hoursToCover : hours
        };
        _ = await _context.AddAsync(loadItem);
        var result = await _context.SaveChangesAsync();
        await RecalculateNonTeachingLoadItemsAsync(teacherId, planItem.PeriodId);
        return result > 0;
    }

    public async Task<bool> DeleteLoadFromTeacherAsync(Guid loadItemId)
    {
        if (await ExistsLoadItemAsync(loadItemId))
        {
            var loadItem = await _context.LoadItems.Where(l => l.Id == loadItemId).Include(l => l.PlanningItem).FirstOrDefaultAsync();
            if (loadItem is not null)
            {
                _ = _context.Remove(loadItem);
                var result = await _context.SaveChangesAsync();
                await RecalculateNonTeachingLoadItemsAsync(loadItem.TeacherId, loadItem.PlanningItem.PeriodId);
                return result > 0;
            }
        }
        throw new LoadItemNotFoundException();
    }

    private async Task<bool> ExistsLoadItemAsync(Guid loadItemId)
        => await _context.LoadItems.AnyAsync(i => i.Id == loadItemId);

    public async Task<IList<TeacherModel>> GetSupportTeachersAsync(Guid departmentId, Guid periodId)
    {
        if (departmentId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(departmentId));
        }
        if (!await ExistDepartmentAsync(departmentId))
        {
            throw new DepartmentNotFoundException();
        }
        var disciplines = from discipline in _context.Disciplines
                          where discipline.DepartmentId == departmentId
                          select discipline;

        var subjects = from subject in _context.Subjects
                       join discipline in disciplines
                       on subject.DisciplineId equals discipline.Id
                       select subject;

        var planItems = from planItem in _context.TeachingPlanItems
                        join subject in subjects
                        on planItem.SubjectId equals subject.Id
                        where planItem.PeriodId == periodId
                        select planItem;

        var loadItems = from loadItem in _context.LoadItems
                        join planItem in planItems
                        on loadItem.PlanningItemId equals planItem.Id
                        select loadItem;

        var teachers = from teacher in _context.Teachers
                       join loadItem in loadItems
                       on teacher.Id equals loadItem.TeacherId
                       where teacher.DepartmentId != departmentId
                       select teacher;

        return await teachers.Include(t => t.TeacherDisciplines).ThenInclude(td => td.Discipline).Include(t => t.Department).ToListAsync();
    }

    #endregion

    #region Subject

    public async Task<bool> CreateSubjectAsync(SubjectModel subject)
    {
        if (subject is not null)
        {
            _ = await _context.Subjects.AddAsync(subject);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(subject));
    }

    public async Task<bool> ExistsSubjectAsync(Guid id)
    {
        var result = await _context.Subjects.AnyAsync(t => t.Id == id);
        return result;
    }

    public async Task<int> GetSubjectsCountAsync() => await _context.Subjects.CountAsync();

    public async Task<IList<SubjectModel>> GetSubjectsAsync(int from, int to)
    {
        var result =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.Subjects.Where(s => s.Active).Skip(from).Take(to).Include(d => d.Discipline).ToListAsync()
            : await _context.Subjects.Where(s => s.Active).Include(d => d.Discipline).ToListAsync();
        return result;
    }

    public async Task<IList<SubjectModel>> GetSubjectsForCourseAsync(Guid courseId)
    {
        var query = from s in _context.Subjects
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
        if (!await ExistsCourseAsync(courseId))
        {
            throw new CourseNotFoundException();
        }
        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }
        try
        {
            var query = from subject in _context.Subjects
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

    public async Task<IList<SubjectModel>> GetSubjectsForCourseNotAssignedInPeriodAsync(Guid courseId, Guid periodId)
    {
        var courseSubjectsQuery = from s in _context.Subjects
                                  join d in _context.Disciplines
                                  on s.DisciplineId equals d.Id
                                  join cd in _context.CurriculumsDisciplines
                                  on d.Id equals cd.DisciplineId
                                  join sy in _context.Courses
                                  on cd.CurriculumId equals sy.CurriculumId
                                  where sy.Id == courseId && s.Active
                                  select s;

        var assignedSubjectsQuery = from periodSubject in _context.PeriodSubjects
                                    join subject in _context.Subjects
                                    on periodSubject.SubjectId equals subject.Id
                                    where periodSubject.CourseId == courseId && periodSubject.PeriodId == periodId
                                    select subject;

        var subjects = courseSubjectsQuery.Except(assignedSubjectsQuery);
        subjects = subjects.Include(s => s.Discipline);

        return await subjects.ToListAsync();
    }

    public async Task<SubjectModel> GetSubjectAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var result = await _context.Subjects.Where(t => t.Id == id)
                                                .Include(s => s.Discipline)
                                                .FirstOrDefaultAsync();
            return result ?? throw new SubjectNotFoundException();
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<bool> UpdateSubjectAsync(SubjectModel subject)
    {
        if (subject is not null)
        {
            _ = _context.Subjects.Update(subject);
            var result = await _context.SaveChangesAsync();
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
                var subject = await GetSubjectAsync(id);
                if (!await SubjectHaveLoad(id))
                {
                    _ = _context.Subjects.Remove(subject);
                }
                else
                {
                    subject.Active = false;
                    _ = _context.Update(subject);
                }
                var result = await _context.SaveChangesAsync();
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
        var query = from planItem in _context.TeachingPlanItems
                    join loadItem in _context.LoadItems
                    on planItem.Id equals loadItem.PlanningItemId
                    where planItem.SubjectId == subjectId
                    select loadItem;

        return await query.CountAsync() > 0;
    }

    public async Task<IList<PeriodSubjectModel>> GetPeriodSubjectsForCourseAsync(Guid periodId, Guid courseId)
    {
        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }
        if (!await ExistsCourseAsync(courseId))
        {
            throw new CourseNotFoundException();
        }
        var query = from periodSubject in _context.PeriodSubjects
                    where periodSubject.PeriodId == periodId && periodSubject.CourseId == courseId
                    select periodSubject;
        query = query.Include(ps => ps.Subject);

        return await query.ToListAsync();
    }

    public async Task<bool> CreatePeriodSubjectAsync(PeriodSubjectModel newPeriodSubject)
    {
        try
        {
            if (!await ExistsPeriodAsync(newPeriodSubject.PeriodId))
            {
                throw new PeriodNotFoundException();
            }
            if (!await ExistsCourseAsync(newPeriodSubject.CourseId))
            {
                throw new CourseNotFoundException();
            }
            if (!await ExistsSubjectAsync(newPeriodSubject.SubjectId))
            {
                throw new SubjectNotFoundException();
            }
            _ = _context.PeriodSubjects.Add(newPeriodSubject);
            var result = await _context.SaveChangesAsync();
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
            var result = await _context.PeriodSubjects.Where(ps => ps.Id == id)
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
            _ = _context.PeriodSubjects.Update(periodSubject);
            var result = await _context.SaveChangesAsync();
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
            var periodSubject = await _context.PeriodSubjects.FirstAsync(ps => ps.Id == id);
            _ = _context.PeriodSubjects.Remove(periodSubject);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region Curriculums

    public async Task<bool> CreateCurriculumAsync(CurriculumModel curriculum)
    {
        if (curriculum is not null)
        {
            _ = await _context.Curriculums.AddAsync(curriculum);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(curriculum));
    }

    public async Task<bool> ExistsCurriculumAsync(Guid id)
    {
        var result = await _context.Curriculums.AnyAsync(t => t.Id == id);
        return result;
    }

    public async Task<int> GetCurriculumsCountAsync() => await _context.Curriculums.CountAsync();

    public async Task<int> GetCurriculumDisciplinesCountAsync(Guid id)
    {
        var result = await _context.CurriculumsDisciplines.CountAsync(td => td.CurriculumId == id);
        return result;
    }

    public async Task<IList<CurriculumModel>> GetCurriculumsAsync(int from, int to)
    {
        var result =
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
            if (!await ExistsCareerAsync(careerId))
            {
                throw new CareerNotFoundException();
            }
            var curriculums = await _context.Curriculums.Where(c => c.CareerId == careerId).ToListAsync();
            return curriculums;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<CurriculumModel> GetCurriculumAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var result = await _context.Curriculums.Where(t => t.Id == id)
                                                   .Include(c => c.Career)
                                                   .Include(c => c.CurriculumDisciplines)
                                                   .ThenInclude(cs => cs.Discipline)
                                                   .FirstOrDefaultAsync();
            return result ?? throw new TeacherNotFoundException();
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<bool> UpdateCurriculumAsync(CurriculumModel curriculum)
    {
        if (curriculum is not null)
        {
            await _context.CurriculumsDisciplines.Where(td => td.CurriculumId == curriculum.Id)
                                              .ForEachAsync(td => _context.Remove(td));
            await _context.CurriculumsDisciplines.AddRangeAsync(curriculum.CurriculumDisciplines);
            _ = _context.Curriculums.Update(curriculum);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(curriculum));
    }

    public async Task<bool> DeleteCurriculumAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            try
            {
                var curriculum = await GetCurriculumAsync(id);
                _ = _context.Curriculums.Remove(curriculum);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (CurriculumNotFoundException)
            {
                throw;
            }
        }
        throw new ArgumentNullException(nameof(id));
    }

    #endregion

    #region SchoolYears

    public async Task<SchoolYearModel> GetCurrentSchoolYear()
    {
        try
        {
            return await _context.SchoolYears.Include(sy => sy.Periods).Where(sy => sy.Current).FirstAsync() ?? throw new NotCurrentSchoolYearDefined();
        }
        catch (NotCurrentSchoolYearDefined)
        {
            throw;
        }
    }

    public async Task<SchoolYearModel> GetSchoolYearAsync(Guid id)
    {
        var schoolYear = await _context.SchoolYears.Include(sy => sy.Periods).FirstOrDefaultAsync(sy => sy.Id.Equals(id));
        return schoolYear is null ? throw new SchoolYearNotFoundException() : schoolYear;
    }

    public async Task<IList<SchoolYearModel>> GetSchoolYearsAsync(int from = 0, int to = 0)
    {
        var schoolYears =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.SchoolYears.Include(sy => sy.Periods).Skip(from).Take(to).ToListAsync()
            : await _context.SchoolYears.Include(sy => sy.Periods).ToListAsync();
        return schoolYears.OrderByDescending(sy => sy.Current).ThenByDescending(sy => sy.Name).ToList();
    }

    public async Task<IList<CourseModel>> GetCoursesAsync(Guid schoolYearId)
    {
        var courses = await _context.Courses.Include(c => c.SchoolYear)
                                            .Include(c => c.Curriculum)
                                            .Where(c => c.SchoolYearId == schoolYearId)
                                            .ToListAsync();
        return courses;
    }

    public async Task<int> GetSchoolYearTotalAsync()
    {
        var total = await _context.SchoolYears.CountAsync();
        return total;
    }

    public async Task<bool> ExistSchoolYearAsync(Guid id)
    {
        var result = await _context.SchoolYears.AnyAsync(f => f.Id == id);
        return result;
    }

    public async Task<bool> CreateSchoolYearAsync(SchoolYearModel schoolYear)
    {
        if (schoolYear is not null)
        {
            if (schoolYear.Current)
            {
                await RemoveCurrentSchoolYearMark();
            }
            _ = await _context.AddAsync(schoolYear);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(schoolYear));
    }

    public async Task<bool> UpdateSchoolYearAsync(SchoolYearModel schoolYear)
    {
        if (schoolYear is not null)
        {
            if (schoolYear.Current)
            {
                await RemoveCurrentSchoolYearMark();
            }
            _ = _context.Update(schoolYear);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(schoolYear));
    }

    public async Task<int> GetSchoolYearCoursesCountAsync(Guid schoolYearId)
    {
        var count = await _context.Courses.CountAsync(c => c.SchoolYearId == schoolYearId);
        return count;
    }

    public async Task<bool> DeleteSchoolYearAsync(Guid schoolYearId)
    {
        try
        {
            var goal = 1;
            var schoolYear = await GetSchoolYearAsync(schoolYearId);
            if (schoolYear.Current && await _context.SchoolYears.CountAsync() > 1)
            {
                var nextCurrent = await _context.SchoolYears.OrderByDescending(sy => sy.Name)
                                                            .FirstOrDefaultAsync(sy => sy.Id != schoolYear.Id);
                if (nextCurrent is not null)
                {
                    nextCurrent.Current = true;
                    _ = _context.SchoolYears.Update(nextCurrent);
                    goal++;
                }
            }
            _ = _context.Remove(schoolYear);
            var result = await _context.SaveChangesAsync();
            return result >= goal;
        }
        catch (SchoolYearNotFoundException)
        {
            throw;
        }
    }

    private async Task RemoveCurrentSchoolYearMark()
    {
        var query = from schoolYear in _context.SchoolYears
                    where schoolYear.Current
                    select schoolYear;
        foreach (var schoolYear in query)
        {
            schoolYear.Current = false;
            _ = _context.Update(schoolYear);
        }
        _ = await _context.SaveChangesAsync();
    }

    #endregion

    #region Courses

    public async Task<bool> CreateCourseAsync(CourseModel course)
    {
        if (course is not null)
        {
            _ = await _context.Courses.AddAsync(course);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(course));
    }

    public async Task<bool> ExistsCourseAsync(Guid id)
    {
        var result = await _context.Courses.AnyAsync(d => d.Id == id);
        return result;
    }

    public async Task<bool> CheckCourseExistenceByCareerYearAndModality(Guid careerId, int careerYear, TeachingModality modality)
    {
        var result = await _context.Courses.AnyAsync(sy => sy.CareerId == careerId && sy.CareerYear == careerYear && sy.TeachingModality == modality);
        return result;
    }

    public async Task<IList<CourseModel>> GetCoursesAsync(int from, int to)
    {
        var result =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.Courses.Skip(from).Take(to).Include(y => y.SchoolYear).Include(y => y.Career).Include(y => y.Curriculum).ToListAsync()
            : await _context.Courses.Include(y => y.SchoolYear).Include(y => y.Career).Include(y => y.Curriculum).ToListAsync();
        return result;
    }

    public async Task<int> GetCoursesCountAsync() => await _context.Courses.CountAsync();

    public async Task<CourseModel> GetCourseAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var result = await _context.Courses.Where(d => d.Id == id)
                                               .Include(y => y.SchoolYear)
                                               .Include(y => y.Career)
                                               .Include(y => y.Curriculum)
                                               .FirstOrDefaultAsync();
            return result ?? throw new CourseNotFoundException();
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<bool> UpdateCourseAsync(CourseModel course)
    {
        if (course is not null)
        {
            _ = _context.Courses.Update(course);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(course));
    }

    public async Task<bool> DeleteCourseAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            try
            {
                var course = await GetCourseAsync(id);
                _ = _context.Courses.Remove(course);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (DisciplineNotFoundException)
            {
                throw;
            }
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<IList<CourseModel>> GetCoursesForDepartment(Guid departmentId, Guid? schoolYearId = null)
    {
        if (departmentId != Guid.Empty)
        {
            var disciplines = from discipline in _context.Disciplines
                              where discipline.DepartmentId == departmentId
                              select discipline;
            var curriculums = from curriculum in _context.Curriculums
                              join curriculumsDisciplines in _context.CurriculumsDisciplines
                              on curriculum.Id equals curriculumsDisciplines.CurriculumId
                              join discipline in disciplines
                              on curriculumsDisciplines.DisciplineId equals discipline.Id
                              select curriculum;
            var courses = from course in _context.Courses
                          join curriculum in curriculums
                          on course.CurriculumId equals curriculum.Id
                          select course;
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
        throw new ArgumentNullException();
    }

    #endregion

    #region Periods

    public async Task<bool> CreatePeriodAsync(PeriodModel period)
    {
        if (period is not null)
        {
            period.TimeFund = _periodCalculator.CalculateValue(period);
            _ = await _context.Periods.AddAsync(period);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(period));
    }

    public async Task<bool> ExistsPeriodAsync(Guid id)
    {
        var result = await _context.Periods.AnyAsync(d => d.Id == id);
        return result;
    }

    public async Task<IList<PeriodModel>> GetPeriodsAsync(int from, int to)
    {
        var result =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.Periods.Skip(from).Take(to).Include(p => p.SchoolYear).ToListAsync()
            : await _context.Periods.Include(p => p.SchoolYear).ToListAsync();
        return result;
    }

    public async Task<int> GetPeriodsCountAsync() => await _context.Periods.CountAsync();

    public async Task<PeriodModel> GetPeriodAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var result = await _context.Periods.Where(p => p.Id == id)
                                               .Include(p => p.SchoolYear)
                                               .FirstOrDefaultAsync();
            return result ?? throw new PeriodNotFoundException();
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<bool> UpdatePeriodAsync(PeriodModel period)
    {
        if (period is not null)
        {
            period.TimeFund = _periodCalculator.CalculateValue(period);
            _ = _context.Periods.Update(period);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(period));
    }

    public async Task<bool> DeletePeriodAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            try
            {
                var period = await GetPeriodAsync(id);
                _ = _context.Periods.Remove(period);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (PeriodNotFoundException)
            {
                throw;
            }
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<double> GetPeriodTimeFund(Guid periodId)
    {
        if (periodId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(periodId));
        }
        if (!await ExistsPeriodAsync(periodId))
        {
            throw new PeriodNotFoundException();
        }
        var period = await _context.Periods.FirstAsync(p => p.Id == periodId);
        return period.TimeFund;
    }

    public async Task<IList<PeriodModel>> GetPeriodsOfSchoolYearAsync(Guid schoolYear)
    {
        var query = from period in _context.Periods
                    where period.SchoolYearId == schoolYear
                    select period;

        return await query.ToListAsync();
    }

    public async Task<int> GetSchoolYearPeriodsCountAsync(Guid schoolYearId)
        => await _context.Periods.CountAsync(p => p.SchoolYearId == schoolYearId);

    public async Task<bool> IsPeriodInCurrentYear(Guid periodId)
    {
        var query = from period in _context.Periods
                    join schoolYear in _context.SchoolYears
                    on period.SchoolYearId equals schoolYear.Id
                    where period.Id == periodId
                    select schoolYear.Current;

        return await query.FirstAsync();
    }

    #endregion

    #region TeachingPlanItems

    public async Task<bool> CreateTeachingPlanItemAsync(TeachingPlanItemModel teachingPlanItem)
    {
        if (teachingPlanItem is not null)
        {
            teachingPlanItem.FromPostgraduateCourse = await IsPostgraduateCourse(teachingPlanItem.CourseId);
            _ = await _context.TeachingPlanItems.AddAsync(teachingPlanItem);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(teachingPlanItem));
    }

    private async Task<bool> IsPostgraduateCourse(Guid courseId)
    {
        var query = from course in _context.Courses
                    join career in _context.Careers
                    on course.CareerId equals career.Id
                    where course.Id == courseId
                    select career.PostgraduateCourse;

        return await query.FirstAsync();
    }

    public async Task<bool> ExistsTeachingPlanItemAsync(Guid id)
    {
        var result = await _context.TeachingPlanItems.AnyAsync(d => d.Id == id);
        return result;
    }

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(int from, int to)
    {
        var result =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.TeachingPlanItems.Skip(from).Take(to).Include(p => p.Subject).Include(p => p.Course).ToListAsync()
            : await _context.TeachingPlanItems.Include(p => p.Subject).Include(p => p.Course).ToListAsync();
        result.ForEach(i => i.TotalHoursPlanned = _planItemCalculator.CalculateValue(i));
        return result;
    }

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(Guid periodId, int from, int to)
    {
        var result =
            (from != 0 && from == to) || (from >= 0 && to >= from && !(from == 0 && from == to))
            ? await _context.TeachingPlanItems.Where(tp => tp.PeriodId == periodId).Skip(from).Take(to).Include(p => p.Subject).Include(p => p.Course).Include(p => p.LoadItems).ThenInclude(i => i.Teacher).ToListAsync()
            : await _context.TeachingPlanItems.Where(tp => tp.PeriodId == periodId).Include(p => p.Subject).Include(p => p.Course).Include(p => p.LoadItems).ThenInclude(i => i.Teacher).ToListAsync();
        result.ForEach(i => i.TotalHoursPlanned = _planItemCalculator.CalculateValue(i));
        return result;
    }

    public async Task<int> GetTeachingPlanItemsCountAsync() => await _context.TeachingPlanItems.CountAsync();

    public async Task<int> GetTeachingPlanItemsCountAsync(Guid periodId) => await _context.TeachingPlanItems.Where(tp => tp.PeriodId == periodId).CountAsync();

    public async Task<TeachingPlanItemModel> GetTeachingPlanItemAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var resultQuery = _context.TeachingPlanItems.Where(p => p.Id == id);
            var result = await _context.TeachingPlanItems.Where(p => p.Id == id)
                                                         .Include(p => p.Subject)
                                                         .Include(p => p.LoadItems)
                                                         .ThenInclude(li => li.Teacher)
                                                         .Include(p => p.Course)
                                                         .FirstOrDefaultAsync();
            if (result is null)
            {
                throw new TeachingPlanItemNotFoundException();
            }
            result.TotalHoursPlanned = _planItemCalculator.CalculateValue(result);
            return result;
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<bool> UpdateTeachingPlanItemAsync(TeachingPlanItemModel teachingPlanItem)
    {
        if (teachingPlanItem is not null)
        {
            _ = _context.TeachingPlanItems.Update(teachingPlanItem);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(teachingPlanItem));
    }

    public async Task<bool> DeleteTeachingPlanItemAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            try
            {
                var teachingPlanItem = await GetTeachingPlanItemAsync(id);
                var loadItemsAffected = await _context.LoadItems.Where(l => l.PlanningItemId == id)
                                                             .Select(l => new { teacherId = l.TeacherId, periodId = teachingPlanItem.PeriodId })
                                                             .ToListAsync();
                _ = _context.TeachingPlanItems.Remove(teachingPlanItem);
                var result = await _context.SaveChangesAsync();
                foreach (var item in loadItemsAffected)
                {
                    await RecalculateAutogenerateTeachingLoadItemsAsync(item.teacherId, item.periodId);
                }
                return result > 0;
            }
            catch (TeachingPlanItemNotFoundException)
            {
                throw;
            }
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsOfDepartmentOnPeriod(Guid departmentId, Guid periodId, Guid? courseId = null)
    {
        try
        {
            if (departmentId == Guid.Empty || periodId == Guid.Empty || !await ExistDepartmentAsync(departmentId) || !await ExistsPeriodAsync(periodId))
            {
                throw new ArgumentNullException();
            }

            var disciplines = from discipline in _context.Disciplines
                              where discipline.DepartmentId == departmentId
                              select discipline;

            var subjects = from subject in _context.Subjects
                           join discipline in disciplines
                           on subject.DisciplineId equals discipline.Id
                           select subject;

            var planItems = from planItem in _context.TeachingPlanItems
                            join subject in subjects
                            on planItem.SubjectId equals subject.Id
                            where planItem.PeriodId == periodId
                            select planItem;

            var items = planItems.Distinct().Include(i => i.Subject).Include(p => p.Course).Include(i => i.LoadItems).ThenInclude(li => li.Teacher);
            var planItemsList = courseId is not null ? await items.Where(i => i.CourseId == courseId).ToListAsync() : await items.ToListAsync();
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
        var query = from planItem in _context.TeachingPlanItems
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
        var query = from loadItem in _context.LoadItems
                    where loadItem.PlanningItemId == planItemId
                    select loadItem;
        return await query.SumAsync(i => i.HoursCovered);
    }

    #endregion

    #region Teachers - Disciplines

    public async Task<IList<DisciplineModel>> GetDisciplinesForTeacher(Guid teacherId)
    {
        if (await ExistsTeacherAsync(teacherId))
        {
            var disciplines = from td in _context.TeachersDisciplines
                              join discipline in _context.Disciplines
                              on td.DisciplineId equals discipline.Id
                              where td.TeacherId == teacherId
                              select discipline;
            disciplines = disciplines.Include(d => d.Department);
            return await disciplines.ToListAsync();
        }
        throw new TeacherNotFoundException();
    }

    #endregion
}