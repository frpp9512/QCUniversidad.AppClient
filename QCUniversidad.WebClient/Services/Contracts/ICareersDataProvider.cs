using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Planning;

namespace QCUniversidad.WebClient.Services.Contracts;

public interface ICareersDataProvider
{
    Task<IList<CareerModel>> GetCareersAsync(int from = 0, int to = 0);
    Task<IList<CareerModel>> GetCareersAsync(Guid facultyId);
    Task<IList<CareerModel>> GetCareersForDepartmentAsync(Guid departmentId);
    Task<int> GetCareersCountAsync();
    Task<CareerModel> GetCareerAsync(Guid careerId);
    Task<bool> ExistsCareerAsync(Guid id);
    Task<bool> CreateCareerAsync(CareerModel newCareer);
    Task<bool> UpdateCareerAsync(CareerModel career);
    Task<bool> DeleteCareerAsync(Guid careerId);
    Task<CoursePeriodPlanningInfoModel> GetCoursePeriodPlanningInfoAsync(Guid courseId, Guid periodId);
}
