using QCUniversidad.AppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.Services.Data
{
    public interface IDataProvider
    {
        #region Faculties

        Task<FacultyModel> GetFacultyAsync(Guid id);
        Task<IList<FacultyModel>> GetFacultiesAsync(int from = 0, int to = 0);
        Task<bool> CreateFacultyAsync(FacultyModel facultyModel);
        Task<bool> UpdateFacultyAsync(FacultyModel facultyModel);
        Task<bool> DeleteFacultyAsync(Guid id);

        #endregion

        #region Deparments

        Task<IList<DepartmentModel>> GetDeparmentsAsync(Guid facultyId);
        Task<DepartmentModel> GetDeparmentAsync(Guid deparmentId);
        Task<bool> CreateDepartmentAsync(DepartmentModel newDepartment);

        #endregion
    }
}
