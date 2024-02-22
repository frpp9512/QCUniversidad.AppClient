using QCUniversidad.WebClient.Models.Curriculums;

namespace QCUniversidad.WebClient.Services.Contracts;

public interface ICurriculumsDataProvider
{
    Task<IList<CurriculumModel>> GetCurriculumsAsync(int from = 0, int to = 0);
    Task<IList<CurriculumModel>> GetCurriculumsForCareerAsync(Guid careerId);
    Task<int> GetCurriculumsCountAsync();
    Task<bool> ExistsCurriculumAsync(Guid id);
    Task<CurriculumModel> GetCurriculumAsync(Guid curriculumId);
    Task<bool> CreateCurriculumAsync(CurriculumModel newCurriculum);
    Task<bool> UpdateCurriculumAsync(CurriculumModel curriculum);
    Task<bool> DeleteCurriculumAsync(Guid curriculumId);
}
