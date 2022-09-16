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
        Task<int> GetFacultiesTotalAsync();
        Task<bool> ExistFacultyAsync(Guid id);
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

        #region Careers

        Task<bool> CreateCareerAsync(CareerModel career);
        Task<IList<CareerModel>> GetCareersAsync(Guid facultyId);
        Task<CareerModel> GetCareerAsync(Guid careerId);
        Task<bool> UpdateCareerAsync(CareerModel career);
        Task<bool> DeleteCareerAsync(Guid careerId);

        #endregion

        #region Disciplines

        Task<bool> CreateDisciplineAsync(DisciplineModel discipline);
        Task<IList<DisciplineModel>> GetDisciplinesAsync();
        Task<DisciplineModel> GetDisciplineAsync(Guid disciplineId);
        Task<bool> UpdateDisciplineAsync(DisciplineModel discipline);
        Task<bool> DeleteDisciplineAsync(Guid disciplineId);

        #endregion
    }
}