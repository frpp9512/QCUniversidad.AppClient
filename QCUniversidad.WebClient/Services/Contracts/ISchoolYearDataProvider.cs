using QCUniversidad.WebClient.Models.SchoolYears;

namespace QCUniversidad.WebClient.Services.Contracts;

public interface ISchoolYearDataProvider
{
    Task<SchoolYearModel> GetCurrentSchoolYear();
    Task<SchoolYearModel> GetSchoolYearAsync(Guid id);
    Task<IList<SchoolYearModel>> GetSchoolYearsAsync(int from = 0, int to = 0);
    Task<int> GetSchoolYearTotalAsync();
    Task<bool> ExistSchoolYearAsync(Guid id);
    Task<bool> CreateSchoolYearAsync(SchoolYearModel schoolYear);
    Task<bool> UpdateSchoolYearAsync(SchoolYearModel schoolYear);
    Task<bool> DeleteSchoolYearAsync(Guid schoolYear);
}
