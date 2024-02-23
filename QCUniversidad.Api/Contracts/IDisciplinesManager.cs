using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Contracts;

public interface IDisciplinesManager
{
    Task<DisciplineModel?> CreateDisciplineAsync(DisciplineModel discipline);
    Task<bool> ExistsDisciplineAsync(Guid id);
    Task<bool> ExistsDisciplineAsync(string name);
    Task<int> GetDisciplinesCountAsync();
    Task<int> GetDisciplineSubjectsCountAsync(Guid disciplineId);
    Task<int> GetDisciplineTeachersCountAsync(Guid disciplineId);
    Task<IList<DisciplineModel>> GetDisciplinesAsync(int from, int to);
    Task<IList<DisciplineModel>> GetDisciplinesAsync(Guid departmentId);
    Task<DisciplineModel> GetDisciplineAsync(Guid disciplineId);
    Task<DisciplineModel> GetDisciplineAsync(string name);
    Task<bool> UpdateDisciplineAsync(DisciplineModel discipline);
    Task<bool> DeleteDisciplineAsync(Guid disciplineId);
    Task<IList<DisciplineModel>> GetDisciplinesForTeacher(Guid teacherId);
}
