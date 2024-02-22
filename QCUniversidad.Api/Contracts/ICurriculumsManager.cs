using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Contracts;

public interface ICurriculumsManager
{
    Task<CurriculumModel> CreateCurriculumAsync(CurriculumModel curriculum);
    Task<bool> ExistsCurriculumAsync(Guid id);
    Task<int> GetCurriculumsCountAsync();
    Task<int> GetCurriculumDisciplinesCountAsync(Guid id);
    Task<IList<CurriculumModel>> GetCurriculumsAsync(int from, int to);
    Task<IList<CurriculumModel>> GetCurriculumsForCareerAsync(Guid careerId);
    Task<CurriculumModel> GetCurriculumAsync(Guid id);
    Task<bool> UpdateCurriculumAsync(CurriculumModel curriculum);
    Task<bool> DeleteCurriculumAsync(Guid id);
}
