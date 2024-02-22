using QCUniversidad.WebClient.Models.Faculties;

namespace QCUniversidad.WebClient.Services.Contracts;

public interface IFacultiesDataProvider
{
    Task<FacultyModel> GetFacultyAsync(Guid id);
    Task<IList<FacultyModel>> GetFacultiesAsync(int from = 0, int to = 0);
    Task<int> GetFacultiesTotalAsync();
    Task<bool> ExistFacultyAsync(Guid id);
    Task<bool> CreateFacultyAsync(FacultyModel facultyModel);
    Task<bool> UpdateFacultyAsync(FacultyModel facultyModel);
    Task<bool> DeleteFacultyAsync(Guid id);
}
