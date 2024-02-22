using QCUniversidad.WebClient.Models.LoadDistribution;
using QCUniversidad.WebClient.Models.LoadItem;
using QCUniversidad.WebClient.Models.Teachers;

namespace QCUniversidad.WebClient.Services.Contracts;

public interface ITeachersDataProvider
{
    Task<IList<TeacherModel>> GetTeachersAsync(int from = 0, int to = 0);
    Task<int> GetTeachersCountAsync();
    Task<bool> ExistsTeacherAsync(Guid id);
    Task<bool> ExistsTeacherAsync(string personalId);
    Task<TeacherModel> GetTeacherAsync(Guid teacherId);
    Task<TeacherModel> GetTeacherAsync(string personalId);
    Task<TeacherModel> GetTeacherAsync(Guid teacherId, Guid periodId);
    Task<bool> CreateTeacherAsync(TeacherModel newTeacher);
    Task<bool> UpdateTeacherAsync(TeacherModel teacher);
    Task<bool> DeleteTeacherAsync(Guid teacherId);
    Task<IList<TeacherModel>> GetTeachersOfDepartmentAsync(Guid departmentId);
    Task<IList<TeacherModel>> GetTeachersOfDepartmentNotAssignedToLoadItemAsync(Guid departmentId, Guid planItemId, Guid? disciplineId = null);
    Task<IList<LoadViewItemModel>> GetTeacherLoadItemsInPeriodAsync(Guid teacherId, Guid periodId);
    Task<bool> SetLoadItemAsync(CreateLoadItemModel newLoadItem);
    Task<IList<TeacherModel>> GetTeachersOfDepartmentForPeriodAsync(Guid departmentId, Guid periodId);
    Task<IList<TeacherModel>> GetTeachersOfDepartmentForPeriodWithLoadItemsAsync(Guid departmentId, Guid periodId);
    Task<bool> DeleteLoadItemAsync(Guid loadItemId);
    Task<IList<TeacherModel>> GetSupportTeachersAsync(Guid departmentId, Guid periodId);
    Task<bool> SetNonTeachingLoadAsync(SetNonTeachingLoadModel model);
    Task<IList<BirthdayTeacherModel>> GetBirthdayTeachersForCurrentWeekAsync(Guid departmentId);
    Task<IList<BirthdayTeacherModel>> GetBirthdayTeachersForCurrentMonthAsync(Guid departmentId);
    Task<IList<BirthdayTeacherModel>> GetBirthdayTeachersForCurrentWeekAsync(Guid scopeId, string scope);
    Task<IList<BirthdayTeacherModel>> GetBirthdayTeachersForCurrentMonthAsync(Guid scopeId, string scope);
}
