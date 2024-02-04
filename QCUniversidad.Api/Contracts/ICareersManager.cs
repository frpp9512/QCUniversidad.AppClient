using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Contracts;

public interface ICareersManager
{
    Task<bool> ExistsCareerAsync(Guid id);
    Task<bool> CreateCareerAsync(CareerModel career);
    Task<IList<CareerModel>> GetCareersAsync(int from = 0, int to = 0);
    Task<IList<CareerModel>> GetCareersAsync(Guid facultyId);
    Task<IList<CareerModel>> GetCareersForDepartmentAsync(Guid departmentId);
    Task<CareerModel> GetCareerAsync(Guid careerId);
    Task<int> GetCareersCountAsync();
    Task<bool> UpdateCareerAsync(CareerModel career);
    Task<bool> DeleteCareerAsync(Guid careerId);
}
