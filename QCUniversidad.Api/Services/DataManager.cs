using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Services
{
    #region Exceptions

    public class FacultyNotFoundException : Exception { }
    public class DeparmentNotFoundException : Exception { }
    public class CareerNotFoundException : Exception { }
    public class DisciplineNotFoundException : Exception { }
    public class TeacherNotFoundException : Exception { }
    public class SubjectNotFoundException : Exception { }
    public class CurriculumNotFoundException : Exception { }
    public class SchoolYearNotFoundException : Exception { }
    public class PeriodNotFoundException : Exception { }

    #endregion

    public class DataManager : IDataManager
    {
        private readonly QCUniversidadContext _context;

        public DataManager(QCUniversidadContext context)
        {
            _context = context;
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
            return department ?? throw new DeparmentNotFoundException();
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
                ? await _context.Teachers.Skip(from).Take(to).Include(d => d.Department).Include(d => d.TeacherDisciplines).ThenInclude(td => td.Discipline).ToListAsync()
                : await _context.Teachers.Include(d => d.Department).Include(d => d.TeacherDisciplines).ThenInclude(td => td.Discipline).ToListAsync();
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
                    _context.Teachers.Remove(teacher);
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
                ? await _context.Subjects.Skip(from).Take(to).Include(d => d.Discipline).ToListAsync()
                : await _context.Subjects.Include(d => d.Discipline).ToListAsync();
            return result;
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
                    _context.Subjects.Remove(subject);
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

        public async Task<bool> CreateSchoolYearAsync(SchoolYearModel schoolYear)
        {
            if (schoolYear is not null)
            {
                await _context.SchoolYears.AddAsync(schoolYear);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            throw new ArgumentNullException(nameof(schoolYear));
        }

        public async Task<bool> ExistsSchoolYearAsync(Guid id)
        {
            var result = await _context.SchoolYears.AnyAsync(d => d.Id == id);
            return result;
        }

        public async Task<bool> CheckSchoolYearExistenceByCareerYearAndModality(Guid careerId, int careerYear, TeachingModality modality)
        {
            var result = await _context.SchoolYears.AnyAsync(sy => sy.CareerId == careerId && sy.CareerYear == careerYear && sy.TeachingModality == modality);
            return result;
        }

        public async Task<IList<SchoolYearModel>> GetSchoolYearsAsync(int from, int to)
        {
            var result =
                (from != 0 && from == to) || (from >= 0 && to >= from) && !(from == 0 && from == to)
                ? await _context.SchoolYears.Skip(from).Take(to).Include(y => y.Career).Include(y => y.Curriculum).Include(y => y.Periods).ToListAsync()
                : await _context.SchoolYears.Include(y => y.Career).Include(y => y.Curriculum).Include(y => y.Periods).ToListAsync();
            return result;
        }

        public async Task<int> GetSchoolYearsCountAsync()
        {
            return await _context.SchoolYears.CountAsync();
        }

        public async Task<int> GetSchoolYearPeriodsCountAsync(Guid schoolYearId)
        {
            var result = await _context.Periods.CountAsync(p => p.SchoolYearId == schoolYearId);
            return result;
        }

        public async Task<SchoolYearModel> GetSchoolYearAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = await _context.SchoolYears.Where(d => d.Id == id)
                                                       .Include(y => y.Career)
                                                       .Include(y => y.Curriculum)
                                                       .Include(y => y.Periods)
                                                       .FirstOrDefaultAsync();
                return result ?? throw new SchoolYearNotFoundException();
            }
            throw new ArgumentNullException(nameof(id));
        }

        public async Task<bool> UpdateSchoolYearAsync(SchoolYearModel schoolYear)
        {
            if (schoolYear is not null)
            {
                _context.SchoolYears.Update(schoolYear);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            throw new ArgumentNullException(nameof(schoolYear));
        }

        public async Task<bool> DeleteSchoolYearAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                try
                {
                    var schoolYear = await GetSchoolYearAsync(id);
                    _context.SchoolYears.Remove(schoolYear);
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

        public async Task<IList<PeriodModel>> GetPeriodsAsync(int from, int to)
        {
            var result =
                (from != 0 && from == to) || (from >= 0 && to >= from) && !(from == 0 && from == to)
                ? await _context.Periods.Skip(from).Take(to).Include(p => p.SchoolYear).ToListAsync()
                : await _context.Periods.Include(p => p.SchoolYear).ToListAsync();
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
}