using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Services
{
    public class FacultyNotFoundException : Exception { }
    public class DeparmentNotFoundException : Exception { }
    public class CareerNotFoundException : Exception { }
    public class DisciplineNotFoundException : Exception { }

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
                (from != 0 && from == to) && (from >= 0 && to >= from)
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
            var deparments = (from != 0 && from == to) && from >= 0 && to >= from
                             ? await _context.Departments.Skip(from).Take(to).Include(d => d.Faculty).ToListAsync()
                             : await _context.Departments.Include(d => d.Faculty).ToListAsync();
            return deparments;
        }

        public async Task<bool> ExistsDisciplineAsync(Guid id)
        {
            var result = await _context.Disciplines.AnyAsync(d => d.Id == id);
            return result;
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

        public async Task<IList<CareerModel>> GetCareersAsync(Guid facultyId)
        {
            if (facultyId != Guid.Empty)
            {
                var result = await _context.Careers.Where(c => c.FacultyId == facultyId).ToListAsync();
                return result;
            }
            throw new ArgumentNullException(nameof(facultyId));
        }

        public async Task<CareerModel> GetCareerAsync(Guid careerId)
        {
            if (careerId != Guid.Empty)
            {
                var result = await _context.Careers.FindAsync(careerId);
                return result ?? throw new CareerNotFoundException();
            }
            throw new ArgumentNullException(nameof(careerId));
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

        public async Task<IList<DisciplineModel>> GetDisciplinesAsync(int from, int to)
        {
            var result = 
                (from != 0 && from == to) && (from >= 0 && to >= from) 
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
                return result ?? throw new CareerNotFoundException();
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
    }
}