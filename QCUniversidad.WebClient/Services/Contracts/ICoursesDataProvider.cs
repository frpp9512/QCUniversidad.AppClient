using QCUniversidad.WebClient.Models.Course;

namespace QCUniversidad.WebClient.Services.Contracts;

public interface ICoursesDataProvider
{
    Task<Guid> CreateCourseAsync(CourseModel course);
    Task<bool> ExistsCourseAsync(Guid id);
    Task<bool> CheckCourseExistenceByCareerYearAndModality(Guid careerId, int careerYear, int modality);
    Task<int> GetCoursesCountAsync();
    Task<IList<CourseModel>> GetCoursesAsync(int from = 0, int to = 0);
    Task<IList<CourseModel>> GetCoursesAsync(Guid schoolYearId);
    Task<IList<CourseModel>> GetCoursesAsync(Guid schoolYearId, Guid facultyId);
    Task<IList<CourseModel>> GetCoursesAsync(Guid careerId, Guid schoolYearId, Guid facultyId);
    Task<CourseModel> GetCourseAsync(Guid id);
    Task<bool> UpdateCourseAsync(CourseModel course);
    Task<bool> DeleteCourseAsync(Guid id);

    Task<IList<CourseModel>> GetCoursesForDepartment(Guid departmentId, Guid? schoolYearId = null);
}
