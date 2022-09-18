using QCUniversidad.WebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Services.Data
{
    public interface IDataProvider
    {
        #region Faculties

        Task<FacultyModel> GetFacultyAsync(Guid id);
        Task<IList<FacultyModel>> GetFacultiesAsync(int from = 0, int to = 0);
        Task<int> GetFacultiesTotalAsync();
        Task<bool> ExistFacultyAsync(Guid id);
        Task<bool> CreateFacultyAsync(FacultyModel facultyModel);
        Task<bool> UpdateFacultyAsync(FacultyModel facultyModel);
        Task<bool> DeleteFacultyAsync(Guid id);

        #endregion

        #region Deparments

        Task<IList<DepartmentModel>> GetDepartmentsAsync(int from, int to);
        Task<IList<DepartmentModel>> GetDepartmentsAsync(Guid facultyId);
        Task<bool> ExistsDepartmentAsync(Guid departmentId);
        Task<int> GetDepartmentsCountAsync();
        Task<int> GetDepartmentDisciplinesCount(Guid departmentId);
        Task<int> GetDepartmentsCountAsync(Guid facultyId);
        Task<DepartmentModel> GetDepartmentAsync(Guid deparmentId);
        Task<bool> CreateDepartmentAsync(DepartmentModel newDepartment);
        Task<bool> UpdateDepartmentAsync(DepartmentModel department);
        Task<bool> DeleteDepartmentAsync(Guid departmentId);

        #endregion

        #region Careers

        Task<IList<CareerModel>> GetCareersAsync(Guid facultyId);
        Task<CareerModel> GetCareerAsync(Guid careerId);
        Task<bool> CreateCareerAsync(CareerModel newCareer);
        Task<bool> UpdateCareerAsync(CareerModel career);
        Task<bool> DeleteCareerAsync(Guid careerId);

        #endregion

        #region Disciplines

        Task<IList<DisciplineModel>> GetDisciplinesAsync();
        Task<DisciplineModel> GetDisciplineAsync(Guid disciplineId);
        Task<bool> CreateDisciplineAsync(DisciplineModel newDiscipline);
        Task<bool> UpdateDisciplineAsync(DisciplineModel discipline);
        Task<bool> DeleteDisciplineAsync(Guid disciplineId);

        #endregion
    }
}