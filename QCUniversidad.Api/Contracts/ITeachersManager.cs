using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Contracts;

public interface ITeachersManager
{
    Task<bool> CreateTeacherAsync(TeacherModel teacher);
    Task<bool> ExistsTeacherAsync(Guid id);
    Task<bool> ExistsTeacherAsync(string personalId);
    Task<int> GetTeachersCountAsync();
    Task<int> GetTeacherDisciplinesCountAsync(Guid id);
    Task<IList<TeacherModel>> GetTeachersAsync(int from = 0, int to = 0);
    Task<TeacherModel> GetTeacherAsync(Guid id);
    Task<TeacherModel> GetTeacherAsync(string personalId);
    Task<bool> UpdateTeacherAsync(TeacherModel teacher);
    Task<bool> DeleteTeacherAsync(Guid id);
    Task<IList<TeacherModel>> GetTeachersOfDepartmentAsync(Guid departmentId, bool loadInactives = false);
    Task<IList<TeacherModel>> GetTeachersOfFacultyAsync(Guid facultyId, bool loadInactives = false);
    Task<IList<TeacherModel>> GetSupportTeachersAsync(Guid departmentId, Guid periodId);
}
