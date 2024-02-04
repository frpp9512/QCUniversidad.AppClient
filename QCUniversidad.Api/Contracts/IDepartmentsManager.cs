using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Contracts;

public interface IDepartmentsManager
{
    Task<IList<DepartmentModel>> GetDepartmentsAsync(int from = 0, int to = 0);
    Task<IList<DepartmentModel>> GetDepartmentsAsync(Guid facultyId);
    Task<bool> ExistDepartmentAsync(Guid id);
    Task<int> GetDepartmentsCountAsync();
    Task<int> GetDepartmentsCountAsync(Guid facultyId);
    Task<int> GetDepartmentDisciplinesCount(Guid departmentId);
    Task<DepartmentModel> GetDepartmentAsync(Guid departmentId);
    Task<int> GetDeparmentTeachersCountAsync(Guid departmentId);
    Task<bool> CreateDepartmentAsync(DepartmentModel department);
    Task<bool> UpdateDeparmentAsync(DepartmentModel department);
    Task<bool> DeleteDeparmentAsync(Guid deparmentId);
    Task<double> GetTotalLoadInPeriodAsync(Guid periodId);
    Task<double> GetDepartmentTotalLoadInPeriodAsync(Guid periodId, Guid departmentId);
    Task<double> GetTotalLoadCoveredInPeriodAsync(Guid periodId);
    Task<double> GetDepartmentTotalLoadCoveredInPeriodAsync(Guid periodId, Guid departmentId);
    Task<double> GetDepartmentAverageTotalLoadCoveredInPeriodAsync(Guid periodId, Guid departmentId);
    Task<double> CalculateRAPAsync(Guid departmentId);
    Task<double> GetDepartmentTotalTimeFund(Guid departmentId, Guid periodId);
}
