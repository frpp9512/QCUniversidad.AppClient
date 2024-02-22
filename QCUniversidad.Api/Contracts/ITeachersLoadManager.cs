using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Contracts;

public interface ITeachersLoadManager
{
    Task<bool> DeleteLoadFromTeacherAsync(Guid loadItemId);
    Task<double> GetTeacherLoadInPeriodAsync(Guid teacherId, Guid periodId);
    Task<IList<LoadItemModel>> GetTeacherLoadItemsInPeriodAsync(Guid teacherId, Guid periodId);
    Task<NonTeachingLoadModel> GetTeacherNonTeachingLoadItemInPeriodAsync(NonTeachingLoadType type, Guid teacherId, Guid periodId);
    Task<IList<NonTeachingLoadModel>> GetTeacherNonTeachingLoadItemsInPeriodAsync(Guid teacherId, Guid periodId);
    Task<IList<TeacherModel>> GetTeachersOfDepartmentNotAssignedToPlanItemAsync(Guid departmentId, Guid planItemId, Guid? disciplineId = null);
    Task<double> GetTeacherTimeFund(Guid teacherId, Guid periodId);
    Task RecalculateAllTeachersLoadInPeriodAsync(Guid periodId);
    Task RecalculateAllTeachersLoadOfDepartmentInPeriodAsync(Guid departmentId, Guid periodId);
    Task RecalculateAllTeachersRelatedToCourseInPeriodAsync(Guid courseId, Guid periodId);
    Task RecalculateAutogenerateTeachingLoadItemsAsync(Guid teacherId, Guid periodId);
    Task<NonTeachingLoadModel> RecalculateTeacherNonTeachingLoadItemInPeriodAsync(NonTeachingLoadType type, Guid teacherId, Guid periodId);
    Task<bool> SetLoadToTeacher(Guid teacherId, Guid planItemId, double hours);
    Task<bool> SetNonTeachingLoadAsync(NonTeachingLoadType type, string baseValue, Guid teacherId, Guid periodId);
}
