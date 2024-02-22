using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Contracts;

public interface IFacultiesManager
{
    Task<FacultyModel> GetFacultyAsync(Guid id);
    Task<IList<FacultyModel>> GetFacultiesAsync(int from = 0, int to = 0);
    Task<int> GetFacultiesTotalAsync();
    Task<bool> ExistFacultyAsync(Guid id);
    Task<bool> CreateFacultyAsync(FacultyModel faculty);
    Task<bool> UpdateFacultyAsync(FacultyModel faculty);
    Task<int> GetFacultyDepartmentCountAsync(Guid facultyId);
    Task<int> GetFacultyCareerCountAsync(Guid facultyId);
    Task<bool> DeleteFacultyAsync(Guid facultyId);
}
