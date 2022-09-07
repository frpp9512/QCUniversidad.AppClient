using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Services
{
    public interface IDataManager
    {
        #region Faculties

        Task<FacultyModel> GetFacultyAsync(Guid id);
        Task<IList<FacultyModel>> GetFacultiesAsync(int from = 0, int to = 0);
        Task<bool> CreateFacultyAsync(FacultyModel faculty);
        Task<bool> UpdateFacultyAsync(FacultyModel faculty);
        Task<int> GetFacultyDepartmentCountAsync(Guid facultyId);
        Task<int> GetFacultyCareerCountAsync(Guid facultyId);
        Task<bool> DeleteFacultyAsync(Guid facultyId);

        #endregion

        #region Deparments

        Task<IList<DepartmentModel>> GetDepartmentsAsync();
        Task<IList<DepartmentModel>> GetDepartmentsAsync(Guid facultyId);
        Task<DepartmentModel> GetDeparmentAsync(Guid departmentId);
        Task<int> GetDeparmentTeachersCountAsync(Guid departmentId);
        Task<bool> CreateDepartmentAsync(DepartmentModel department);
        Task<bool> UpdateDeparmentAsync(DepartmentModel department);
        Task<bool> DeleteDeparmentAsync(Guid deparmentId);

        #endregion
    }
}