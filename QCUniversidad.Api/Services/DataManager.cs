using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Migrations;
using QCUniversidad.Api.Shared.Enums;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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

#endregion

public class DataManager : IDataManager
{
    private readonly QCUniversidadContext _context;
    private readonly ICoefficientCalculator<TeachingPlanItemModel> _planItemCalculator;

    public DataManager(QCUniversidadContext context, ICoefficientCalculator<TeachingPlanItemModel> planItemCalculator)
    {
        _context = context;
        _planItemCalculator = planItemCalculator;
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
            (from != 0 && from == to) || (from >= 0 && to >= from) && !(from == 0 && from == to)
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
            await _context.AddAsync(faculty);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(faculty));
    }

    public async Task<bool> UpdateFacultyAsync(FacultyModel faculty)
    {
        if (faculty is not null)
        {
            _context.Update(faculty);
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
            _context.Remove(faculty);
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
        var deparments = (from != 0 && from == to) || from >= 0 && to >= from && !(from == 0 && from == to)
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
        throw new NotImplementedException();
    }

    public async Task<bool> CreateDepartmentAsync(DepartmentModel department)
    {
        if (department is null)
        {
            throw new ArgumentNullException(nameof(department));
        }
        await _context.Departments.AddAsync(department);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateDeparmentAsync(DepartmentModel department)
    {
        if (department is null)
        {
            throw new ArgumentNullException(nameof(department));
        }
        _context.Departments.Update(department);
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
        _context.Departments.Remove(department);
        var result = await _context.SaveChangesAsync();
        return result > 0;
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
            await _context.Careers.AddAsync(career);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(career));
    }

    public async Task<IList<CareerModel>> GetCareersAsync(int from = 0, int to = 0)
    {
        var result = (from != 0 && from == to) || from >= 0 && to >= from && !(from == 0 && from == to)
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
            _context.Careers.Update(career);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var query = from planItem in _context.TeachingPlanItems
                            join period in _context.Periods
                            on planItem.PeriodId equals period.Id
                            join course in _context.Courses
                            on period.CourseId equals course.Id
                            join c in _context.Careers
                            on course.CareerId equals career.Id
                            where c.Id == career.Id && planItem.FromPostgraduateCourse != career.PostgraduateCourse
                            select planItem;
                if (query.Any())
                {
                    await query.ForEachAsync(i => 
                    { 
                        i.FromPostgraduateCourse = career.PostgraduateCourse;
                        _context.Update(i);
                    });
                    await _context.SaveChangesAsync();
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
                _context.Careers.Remove(career);
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
            await _context.Disciplines.AddAsync(discipline);
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
            (from != 0 && from == to) || (from >= 0 && to >= from) && !(from == 0 && from == to)
            ? await _context.Disciplines.Skip(from).Take(to).Include(d => d.Department).ToListAsync()
            : await _context.Disciplines.Include(d => d.Department).ToListAsync();
        return result;
    }

    public async Task<int> GetDisciplinesCountAsync()
    {
        return await _context.Disciplines.CountAsync();
    }

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
            _context.Disciplines.Update(discipline);
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
                _context.Disciplines.Remove(discipline);
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
            await _context.Teachers.AddAsync(teacher);
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

    public async Task<int> GetTeachersCountAsync()
    {
        return await _context.Teachers.CountAsync();
    }

    public async Task<int> GetTeacherDisciplinesCountAsync(Guid id)
    {
        var result = await _context.TeachersDisciplines.CountAsync(td => td.TeacherId == id);
        return result;
    }

    public async Task<IList<TeacherModel>> GetTeachersAsync(int from, int to)
    {
        var result =
            (from != 0 && from == to) && (from >= 0 && to >= from) && !(from == 0 && from == to)
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
            _context.Teachers.Update(teacher);
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
                    _context.Teachers.Remove(teacher);
                }
                else
                {
                    teacher.Active = false;
                    _context.Teachers.Update(teacher);
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
                        where t.DepartmentId == departmentId
                        select t;
            return await query.Include(t => t.TeacherDisciplines).ThenInclude(td => td.Discipline).ToListAsync();
        }
        catch (Exception)
        {
            throw;
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

        return await query.SumAsync(i => i.HoursCovered);
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
        var depTeachersQuery = from teacher in _context.Teachers
                               join teacherDiscipline in _context.TeachersDisciplines
                               on teacher.Id equals teacherDiscipline.TeacherId
                               where teacher.DepartmentId == departmentId
                                     && disciplineId.HasValue 
                                        ? teacherDiscipline.DisciplineId == disciplineId : true
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
                                         select teacher;

        var finalQuery = depTeachersQuery.Except(depTeachersInPlanItemQuery);

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
        await _context.AddAsync(loadItem);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteLoadFromTeacherAsync(Guid loadItemId)
    {
        if (await ExistsLoadItemAsync(loadItemId))
        {
            var loadItem = await _context.LoadItems.FindAsync(loadItemId);
            if (loadItem is not null)
            {
                _context.Remove(loadItem);
                var result = await _context.SaveChangesAsync();
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
            await _context.Subjects.AddAsync(subject);
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

    public async Task<int> GetSubjectsCountAsync()
    {
        return await _context.Subjects.CountAsync();
    }

    public async Task<IList<SubjectModel>> GetSubjectsAsync(int from, int to)
    {
        var result =
            (from != 0 && from == to) || (from >= 0 && to >= from) && !(from == 0 && from == to)
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
            _context.Subjects.Update(subject);
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
                    _context.Subjects.Remove(subject);
                }
                else
                {
                    subject.Active = false;
                    _context.Update(subject);
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

    #endregion

    #region Curriculums

    public async Task<bool> CreateCurriculumAsync(CurriculumModel curriculum)
    {
        if (curriculum is not null)
        {
            await _context.Curriculums.AddAsync(curriculum);
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

    public async Task<int> GetCurriculumsCountAsync()
    {
        return await _context.Curriculums.CountAsync();
    }

    public async Task<int> GetCurriculumDisciplinesCountAsync(Guid id)
    {
        var result = await _context.CurriculumsDisciplines.CountAsync(td => td.CurriculumId == id);
        return result;
    }

    public async Task<IList<CurriculumModel>> GetCurriculumsAsync(int from, int to)
    {
        var result =
            (from != 0 && from == to) || (from >= 0 && to >= from) && !(from == 0 && from == to)
            ? await _context.Curriculums.Skip(from).Take(to).Include(c => c.Career).Include(c => c.CurriculumDisciplines).ThenInclude(cs => cs.Discipline).ToListAsync()
            : await _context.Curriculums.Include(c => c.Career).Include(c => c.CurriculumDisciplines).ThenInclude(cs => cs.Discipline).ToListAsync();
        return result;
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
            _context.Curriculums.Update(curriculum);
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
                _context.Curriculums.Remove(curriculum);
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
            var id = await GetCurrentSchoolYearId();
            return await _context.SchoolYears.FindAsync(id) ?? throw new NotCurrentSchoolYearDefined();
        }
        catch (NotCurrentSchoolYearDefined)
        {
            throw;
        }
    }

    private async Task<Guid> GetCurrentSchoolYearId()
    {
        var query = from schoolYear in _context.SchoolYears
                    where schoolYear.Current
                    select schoolYear.Id;
        return await query.AnyAsync() ? await query.FirstAsync() : throw new NotCurrentSchoolYearDefined();
    }

    public async Task<SchoolYearModel> GetSchoolYearAsync(Guid id)
    {
        var schoolYear = await _context.SchoolYears.FirstOrDefaultAsync(sy => sy.Id.Equals(id));
        return schoolYear is null ? throw new SchoolYearNotFoundException() : schoolYear;
    }

    public async Task<IList<SchoolYearModel>> GetSchoolYearsAsync(int from = 0, int to = 0)
    {
        var schoolYears =
            (from != 0 && from == to) || (from >= 0 && to >= from) && !(from == 0 && from == to)
            ? await _context.SchoolYears.Skip(from).Take(to).ToListAsync()
            : await _context.SchoolYears.ToListAsync();
        return schoolYears.OrderByDescending(sy => sy.Current).ThenByDescending(sy => sy.Name).ToList();
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
            await _context.AddAsync(schoolYear);
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
            _context.Update(schoolYear);
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
                    _context.SchoolYears.Update(nextCurrent);
                    goal++;
                }
            }
            _context.Remove(schoolYear);
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
            _context.Update(schoolYear);
        }
        await _context.SaveChangesAsync();
    }

    #endregion

    #region Courses

    public async Task<bool> CreateCourseAsync(CourseModel course)
    {
        if (course is not null)
        {
            await _context.Courses.AddAsync(course);
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
            (from != 0 && from == to) || (from >= 0 && to >= from) && !(from == 0 && from == to)
            ? await _context.Courses.Skip(from).Take(to).Include(y => y.SchoolYear).Include(y => y.Career).Include(y => y.Curriculum).Include(y => y.Periods).ToListAsync()
            : await _context.Courses.Include(y => y.SchoolYear).Include(y => y.Career).Include(y => y.Curriculum).Include(y => y.Periods).ToListAsync();
        return result;
    }

    public async Task<int> GetCoursesCountAsync()
    {
        return await _context.Courses.CountAsync();
    }

    public async Task<int> GetCoursePeriodsCountAsync(Guid courseId)
    {
        var result = await _context.Periods.CountAsync(p => p.CourseId == courseId);
        return result;
    }

    public async Task<CourseModel> GetCourseAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var result = await _context.Courses.Where(d => d.Id == id)
                                               .Include(y => y.SchoolYear)
                                               .Include(y => y.Career)
                                               .Include(y => y.Curriculum)
                                               .Include(y => y.Periods)
                                               .FirstOrDefaultAsync();
            return result ?? throw new CourseNotFoundException();
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<bool> UpdateCourseAsync(CourseModel course)
    {
        if (course is not null)
        {
            _context.Courses.Update(course);
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
                _context.Courses.Remove(course);
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
            await _context.Periods.AddAsync(period);
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

    public async Task<bool> ExistPeriodWithOrder(Guid courseId, int order)
    {
        var result = await _context.Periods.AnyAsync(p => p.CourseId == courseId && p.OrderNumber == order);
        return result;
    }

    public async Task<IList<PeriodModel>> GetPeriodsAsync(int from, int to)
    {
        var result =
            (from != 0 && from == to) || (from >= 0 && to >= from) && !(from == 0 && from == to)
            ? await _context.Periods.Skip(from).Take(to).Include(p => p.Course).ToListAsync()
            : await _context.Periods.Include(p => p.Course).ToListAsync();
        return result;
    }

    public async Task<int> GetPeriodsCountAsync()
    {
        return await _context.Periods.CountAsync();
    }

    public async Task<PeriodModel> GetPeriodAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var result = await _context.Periods.Where(p => p.Id == id)
                                               .Include(p => p.Course)
                                               .FirstOrDefaultAsync();
            return result ?? throw new PeriodNotFoundException();
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<bool> UpdatePeriodAsync(PeriodModel period)
    {
        if (period is not null)
        {
            _context.Periods.Update(period);
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
                _context.Periods.Remove(period);
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

    public async Task<IList<PeriodModel>> GetPeriodsOfCourseForDepartment(Guid courseId, Guid departmentId)
    {
        if (courseId == Guid.Empty || departmentId == Guid.Empty)
        {
            throw new ArgumentNullException();
        }
        if (!await ExistsCourseAsync(courseId))
        {
            throw new CourseNotFoundException();
        }
        if (!await ExistDepartmentAsync(departmentId))
        {
            throw new DepartmentNotFoundException();
        }
        try
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
                          where course.Id == courseId
                          select course;

            var periods = from period in _context.Periods
                          join course in courses
                          on period.CourseId equals course.Id
                          select period;

            periods = periods.Distinct();
            return await periods.ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
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

    #endregion

    #region TeachingPlanItems

    public async Task<bool> CreateTeachingPlanItemAsync(TeachingPlanItemModel teachingPlanItem)
    {
        if (teachingPlanItem is not null)
        {
            teachingPlanItem.FromPostgraduateCourse = await IsPeriodFromPostgraduateCourse(teachingPlanItem.PeriodId);
            await _context.TeachingPlanItems.AddAsync(teachingPlanItem);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        throw new ArgumentNullException(nameof(teachingPlanItem));
    }

    private async Task<bool> IsPeriodFromPostgraduateCourse(Guid periodId)
    {
        var query = from period in _context.Periods
                    join course in _context.Courses
                    on period.CourseId equals course.Id
                    join career in _context.Careers
                    on course.CareerId equals career.Id
                    where period.Id == periodId
                    select career.PostgraduateCourse;

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsTeachingPlanItemAsync(Guid id)
    {
        var result = await _context.TeachingPlanItems.AnyAsync(d => d.Id == id);
        return result;
    }

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(int from, int to)
    {
        var result =
            (from != 0 && from == to) || (from >= 0 && to >= from) && !(from == 0 && from == to)
            ? await _context.TeachingPlanItems.Skip(from).Take(to).Include(p => p.Subject).ToListAsync()
            : await _context.TeachingPlanItems.Include(p => p.Subject).ToListAsync();
        result.ForEach(i => i.TotalHoursPlanned = _planItemCalculator.CalculateValue(i));
        return result;
    }

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(Guid periodId, int from, int to)
    {
        var result =
            (from != 0 && from == to) || (from >= 0 && to >= from) && !(from == 0 && from == to)
            ? await _context.TeachingPlanItems.Where(tp => tp.PeriodId == periodId).Skip(from).Take(to).Include(p => p.Subject).Include(p => p.LoadItems).ThenInclude(i => i.Teacher).ToListAsync()
            : await _context.TeachingPlanItems.Where(tp => tp.PeriodId == periodId).Include(p => p.Subject).Include(p => p.LoadItems).ThenInclude(i => i.Teacher).ToListAsync();
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
        if (id != Guid.Empty)
        {
            var resultQuery = _context.TeachingPlanItems.Where(p => p.Id == id);
            var result = await _context.TeachingPlanItems.Where(p => p.Id == id)
                                                         .Include(p => p.Subject)
                                                         .Include(p => p.LoadItems)
                                                         .ThenInclude(li => li.Teacher)
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
            _context.TeachingPlanItems.Update(teachingPlanItem);
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
                _context.TeachingPlanItems.Remove(teachingPlanItem);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (TeachingPlanItemNotFoundException)
            {
                throw;
            }
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsOfDepartmentOnPeriod(Guid departmentId, Guid periodId)
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

            var items = await planItems.Distinct().Include(i => i.Subject).Include(i => i.LoadItems).ThenInclude(li => li.Teacher).ToListAsync();
            items.ForEach(i => i.TotalHoursPlanned = _planItemCalculator.CalculateValue(i));
            return items;
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
                    join period in _context.Periods
                    on planItem.PeriodId equals period.Id
                    join course in _context.Courses
                    on period.CourseId equals course.Id
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