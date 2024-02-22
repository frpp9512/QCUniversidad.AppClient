using QCUniversidad.WebClient.Models.Subjects;

namespace QCUniversidad.WebClient.Services.Contracts;

public interface ISubjectsDataProvider
{
    Task<IList<SubjectModel>> GetSubjectsAsync(int from, int to);
    Task<IList<SubjectModel>> GetSubjectsForDisciplineAsync(Guid disciplineId);
    Task<IList<SubjectModel>> GetSubjectsForCourseAsync(Guid courseId);
    Task<IList<SubjectModel>> GetSubjectsForCourseInPeriodAsync(Guid courseId, Guid periodId);
    Task<IList<SubjectModel>> GetSubjectsForCourseNotAssignedInPeriodAsync(Guid courseId, Guid periodId);
    Task<int> GetSubjectsCountAsync();
    Task<bool> ExistsSubjectAsync(Guid id);
    Task<bool> ExistsSubjectAsync(string name);
    Task<SubjectModel> GetSubjectAsync(Guid subjectId);
    Task<SubjectModel> GetSubjectAsync(string name);
    Task<bool> CreateSubjectAsync(SubjectModel newSubject);
    Task<bool> UpdateSubjectAsync(SubjectModel subject);
    Task<bool> DeleteSubjectAsync(Guid subjectId);
    Task<IList<PeriodSubjectModel>> GetPeriodSubjectsForCourseAsync(Guid periodId, Guid courseId);
    Task<bool> CreatePeriodSubjectAsync(PeriodSubjectModel newPeriodSubject);
    Task<PeriodSubjectModel> GetPeriodSubjectAsync(Guid id);
    Task<bool> UpdatePeriodSubjectAsync(PeriodSubjectModel model);
    Task<bool> DeletePeriodSubjectAsync(Guid id);
}
