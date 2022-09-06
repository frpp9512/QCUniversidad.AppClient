using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Data.Context;
using QCUniversidad.Api.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Services
{
    public class FacultyNotFoundException : Exception { }
    public class DeparmentNotFoundException : Exception { }

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
                from >= 0 && to > from 
                ? await _context.Faculties.Skip(from).Take(to).ToListAsync() 
                : await _context.Faculties.ToListAsync();
            return faculties;
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

        public async Task<IList<DepartmentModel>> GetDepartmentsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IList<DepartmentModel>> GetDepartmentsAsync(Guid facultyId)
        {
            var deparments = await _context.Departments.Where(d => d.FacultyId == facultyId).ToListAsync();
            return deparments;
        }

        public async Task<DepartmentModel> GetDeparmentAsync(Guid departmentId)
        {
            var department = await _context.Departments.FindAsync(departmentId);
            return department ?? throw new DeparmentNotFoundException();
        }

        public async Task<int> GetDeparmentTeachersCount(Guid departmentId)
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

        public async Task<bool> UpdateDeparment(DepartmentModel department)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteDeparment(Guid deparmentId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}