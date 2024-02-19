using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Statistics;

namespace QCUniversidad.WebClient.Services.Contracts;

public interface IDepartmentsDataProvider
{
    Task<IList<DepartmentModel>> GetDepartmentsAsync(int from = 0, int to = 0);
    Task<IList<DepartmentModel>> GetDepartmentsAsync(Guid facultyId);
    Task<IList<DepartmentModel>> GetDepartmentsWithLoadAsync(Guid periodId);
    Task<bool> ExistsDepartmentAsync(Guid departmentId);
    Task<int> GetDepartmentsCountAsync();
    Task<int> GetDepartmentDisciplinesCount(Guid departmentId);
    Task<int> GetDepartmentsCountAsync(Guid facultyId);
    Task<DepartmentModel> GetDepartmentAsync(Guid deparmentId);
    Task<bool> CreateDepartmentAsync(DepartmentModel newDepartment);
    Task<bool> UpdateDepartmentAsync(DepartmentModel department);
    Task<bool> DeleteDepartmentAsync(Guid departmentId);
    Task<IList<StatisticItemModel>> GetDepartmentPeriodStatsAsync(Guid departmentId, Guid periodId);
}
