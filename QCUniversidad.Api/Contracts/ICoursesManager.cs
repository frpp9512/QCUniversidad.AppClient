using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Contracts;

public interface ICoursesManager
{
    Task<CourseModel> CreateCourseAsync(CourseModel course);
    Task<bool> ExistsCourseAsync(Guid id);
    Task<bool> CheckCourseExistenceByCareerYearAndModality(Guid careerId, int careerYear, TeachingModality modality);
    Task<int> GetCoursesCountAsync();
    Task<IList<CourseModel>> GetCoursesAsync(int from, int to);
    Task<IList<CourseModel>> GetCoursesAsync(Guid schoolYearId);
    Task<IList<CourseModel>> GetCoursesAsync(Guid schoolYearId, Guid facultyId);
    Task<IList<CourseModel>> GetCoursesAsync(Guid careerId, Guid schoolYearId, Guid facultyId);
    Task<CourseModel> GetCourseAsync(Guid id);
    Task<bool> UpdateCourseAsync(CourseModel course);
    Task<bool> DeleteCourseAsync(Guid id);

    Task<IList<CourseModel>> GetCoursesForDepartmentAsync(Guid departmentId, Guid? schoolYearId = null);

    Task<double> GetHoursPlannedInPeriodForCourseAsync(Guid courseId, Guid periodId);
    Task<double> GetTotalHoursInPeriodForCourseAsync(Guid courseId, Guid periodId);
    Task<double> GetRealHoursPlannedInPeriodForCourseAsync(Guid courseId, Guid periodId);
}
