using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Contracts;

public interface ISubjectsManager
{
    Task<bool> CreateSubjectAsync(SubjectModel subject);
    Task<bool> ExistsSubjectAsync(Guid id);
    Task<bool> ExistsSubjectAsync(string name);
    Task<IList<SubjectModel>> GetSubjectsForDisciplineAsync(Guid disciplineId);
    Task<IList<SubjectModel>> GetSubjectsForCourseAsync(Guid courseId);
    Task<IList<SubjectModel>> GetSubjectsForCourseInPeriodAsync(Guid courseId, Guid periodId);
    Task<IList<SubjectModel>> GetSubjectsForCourseNotAssignedInPeriodAsync(Guid courseId, Guid periodId);
    Task<int> GetSubjectsCountAsync();
    Task<IList<SubjectModel>> GetSubjectsAsync(int from, int to);
    Task<SubjectModel> GetSubjectAsync(Guid id);
    Task<SubjectModel> GetSubjectAsync(string name);
    Task<bool> UpdateSubjectAsync(SubjectModel subject);
    Task<bool> DeleteSubjectAsync(Guid id);
    Task<IList<PeriodSubjectModel>> GetPeriodSubjectsForCourseAsync(Guid periodId, Guid courseId);
    Task<bool> CreatePeriodSubjectAsync(PeriodSubjectModel newPeriodSubject);
    Task<PeriodSubjectModel> GetPeriodSubjectAsync(Guid id);
    Task<bool> ExistsPeriodSubjectAsync(Guid id);
    Task<bool> UpdatePeriodSubjectAsync(PeriodSubjectModel periodSubject);
    Task<bool> DeletePeriodSubjectAsync(Guid id);
}
