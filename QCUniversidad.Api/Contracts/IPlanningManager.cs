using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Contracts;

public interface IPlanningManager
{
    Task<bool> CreateTeachingPlanItemAsync(TeachingPlanItemModel item);
    Task<bool> ExistsTeachingPlanItemAsync(Guid id);
    Task<int> GetTeachingPlanItemsCountAsync();
    Task<int> GetTeachingPlanItemsCountAsync(Guid periodId);
    Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(int from, int to);
    Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(Guid periodId, Guid? courseId = null, int from = 0, int to = 0);
    Task<TeachingPlanItemModel> GetTeachingPlanItemAsync(Guid id);
    Task<bool> UpdateTeachingPlanItemAsync(TeachingPlanItemModel period);
    Task<bool> DeleteTeachingPlanItemAsync(Guid id);

    Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsOfDepartmentOnPeriod(Guid departmentId, Guid periodId, Guid? courseId = null, bool onlyLoadItems = false);
    Task<bool> IsTeachingPlanFromPostgraduateCourse(Guid teachingPlanId);
    Task<double> GetPlanItemTotalCoveredAsync(Guid planItemId);
}
