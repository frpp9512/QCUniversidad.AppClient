using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Contracts;

public interface ISchoolYearsManager
{
    Task<SchoolYearModel> GetCurrentSchoolYearAsync();
    Task<SchoolYearModel> GetSchoolYearAsync(Guid id);
    Task<IList<SchoolYearModel>> GetSchoolYearsAsync(int from = 0, int to = 0);
    Task<int> GetSchoolYearTotalAsync();
    Task<bool> ExistSchoolYearAsync(Guid id);
    Task<bool> CreateSchoolYearAsync(SchoolYearModel schoolYear);
    Task<bool> UpdateSchoolYearAsync(SchoolYearModel schoolYear);
    Task<int> GetSchoolYearCoursesCountAsync(Guid schoolYear);
    Task<bool> DeleteSchoolYearAsync(Guid schoolYear);
}
