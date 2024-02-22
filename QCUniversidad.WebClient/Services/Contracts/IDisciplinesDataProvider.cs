using QCUniversidad.WebClient.Models.Disciplines;

namespace QCUniversidad.WebClient.Services.Contracts;

public interface IDisciplinesDataProvider
{
    Task<IList<DisciplineModel>> GetDisciplinesAsync(int from = 0, int to = 0);
    Task<IList<DisciplineModel>> GetDisciplinesAsync(Guid departmentId);
    Task<int> GetDisciplinesCountAsync();
    Task<bool> ExistsDisciplineAsync(Guid id);
    Task<bool> ExistsDisciplineAsync(string name);
    Task<DisciplineModel> GetDisciplineAsync(Guid disciplineId);
    Task<DisciplineModel> GetDisciplineAsync(string name);
    Task<bool> CreateDisciplineAsync(DisciplineModel newDiscipline);
    Task<bool> UpdateDisciplineAsync(DisciplineModel discipline);
    Task<bool> DeleteDisciplineAsync(Guid disciplineId);
}
