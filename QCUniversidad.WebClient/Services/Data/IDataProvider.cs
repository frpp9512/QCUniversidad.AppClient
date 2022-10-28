using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Courses;
using QCUniversidad.WebClient.Models.Curriculums;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.LoadDistribution;
using QCUniversidad.WebClient.Models.LoadItem;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Models.SchoolYears;
using QCUniversidad.WebClient.Models.Statistics;
using QCUniversidad.WebClient.Models.Subjects;
using QCUniversidad.WebClient.Models.Teachers;

namespace QCUniversidad.WebClient.Services.Data;

public interface IDataProvider
{
    #region Faculties

    Task<FacultyModel> GetFacultyAsync(Guid id);
    Task<IList<FacultyModel>> GetFacultiesAsync(int from = 0, int to = 0);
    Task<int> GetFacultiesTotalAsync();
    Task<bool> ExistFacultyAsync(Guid id);
    Task<bool> CreateFacultyAsync(FacultyModel facultyModel);
    Task<bool> UpdateFacultyAsync(FacultyModel facultyModel);
    Task<bool> DeleteFacultyAsync(Guid id);

    #endregion

    #region Deparments

    Task<IList<DepartmentModel>> GetDepartmentsAsync(int from = 0, int to = 0);
    Task<IList<DepartmentModel>> GetDepartmentsAsync(Guid facultyId);
    Task<IList<DepartmentModel>> GetDepartmentsWithLoadAsync(Guid periodId);
    Task<bool> ExistsDepartmentAsync(Guid departmentId);
    Task<int> GetDepartmentsCountAsync();
    Task<int> GetDepartmentDisciplinesCount(Guid departmentId);
    Task<int> GetDepartmentsCountAsync(Guid facultyId);
    Task<DepartmentModel> GetDepartmentAsync(Guid deparmentId);
    Task<bool> CreateDepartmentAsync(DepartmentModel newDepartment);
    Task<bool> UpdateDepartmentAsync(DepartmentModel department);
    Task<bool> DeleteDepartmentAsync(Guid departmentId);

    #endregion

    #region Careers

    Task<IList<CareerModel>> GetCareersAsync(int from = 0, int to = 0);
    Task<IList<CareerModel>> GetCareersAsync(Guid facultyId);
    Task<int> GetCareersCountAsync();
    Task<CareerModel> GetCareerAsync(Guid careerId);
    Task<bool> ExistsCareerAsync(Guid id);
    Task<bool> CreateCareerAsync(CareerModel newCareer);
    Task<bool> UpdateCareerAsync(CareerModel career);
    Task<bool> DeleteCareerAsync(Guid careerId);

    #endregion

    #region Disciplines

    Task<IList<DisciplineModel>> GetDisciplinesAsync(int from = 0, int to = 0);
    Task<int> GetDisciplinesCountAsync();
    Task<bool> ExistsDisciplineAsync(Guid id);
    Task<DisciplineModel> GetDisciplineAsync(Guid disciplineId);
    Task<bool> CreateDisciplineAsync(DisciplineModel newDiscipline);
    Task<bool> UpdateDisciplineAsync(DisciplineModel discipline);
    Task<bool> DeleteDisciplineAsync(Guid disciplineId);

    #endregion

    #region Teachers

    Task<IList<TeacherModel>> GetTeachersAsync(int from = 0, int to = 0);
    Task<int> GetTeachersCountAsync();
    Task<bool> ExistsTeacherAsync(Guid id);
    Task<TeacherModel> GetTeacherAsync(Guid teacherId);
    Task<bool> CreateTeacherAsync(TeacherModel newTeacher);
    Task<bool> UpdateTeacherAsync(TeacherModel teacher);
    Task<bool> DeleteTeacherAsync(Guid teacherId);
    Task<IList<TeacherModel>> GetTeachersOfDepartmentAsync(Guid departmentId);
    Task<IList<TeacherModel>> GetTeachersOfDepartmentNotAssignedToLoadItemAsync(Guid departmentId, Guid planItemId, Guid? disciplineId = null);
    Task<IList<LoadViewItemModel>> GetTeacherLoadItemsInPeriodAsync(Guid teacherId, Guid periodId);
    Task<bool> SetLoadItemAsync(CreateLoadItemModel newLoadItem);
    Task<IList<TeacherModel>> GetTeachersOfDepartmentForPeriodAsync(Guid departmentId, Guid periodId);
    Task<bool> DeleteLoadItemAsync(Guid loadItemId);
    Task<IList<TeacherModel>> GetSupportTeachersAsync(Guid departmentId, Guid periodId);

    #endregion

    #region Subjects

    Task<IList<SubjectModel>> GetSubjectsAsync(int from, int to);
    Task<IList<SubjectModel>> GetSubjectsForCourseAsync(Guid courseId);
    Task<IList<SubjectModel>> GetSubjectsForCourseInPeriodAsync(Guid courseId, Guid periodId);
    Task<IList<SubjectModel>> GetSubjectsForCourseNotAssignedInPeriodAsync(Guid courseId, Guid periodId);
    Task<int> GetSubjectsCountAsync();
    Task<bool> ExistsSubjectAsync(Guid id);
    Task<SubjectModel> GetSubjectAsync(Guid subjectId);
    Task<bool> CreateSubjectAsync(SubjectModel newSubject);
    Task<bool> UpdateSubjectAsync(SubjectModel subject);
    Task<bool> DeleteSubjectAsync(Guid subjectId);
    Task<IList<PeriodSubjectModel>> GetPeriodSubjectsForCourseAsync(Guid periodId, Guid courseId);
    Task<bool> CreatePeriodSubjectAsync(PeriodSubjectModel newPeriodSubject);
    Task<PeriodSubjectModel> GetPeriodSubjectAsync(Guid id);
    Task<bool> UpdatePeriodSubjectAsync(PeriodSubjectModel model);
    Task<bool> DeletePeriodSubjectAsync(Guid id);

    #endregion

    #region Curriculums

    Task<IList<CurriculumModel>> GetCurriculumsAsync(int from = 0, int to = 0);
    Task<IList<CurriculumModel>> GetCurriculumsForCareerAsync(Guid careerId);
    Task<int> GetCurriculumsCountAsync();
    Task<bool> ExistsCurriculumAsync(Guid id);
    Task<CurriculumModel> GetCurriculumAsync(Guid curriculumId);
    Task<bool> CreateCurriculumAsync(CurriculumModel newCurriculum);
    Task<bool> UpdateCurriculumAsync(CurriculumModel curriculum);
    Task<bool> DeleteCurriculumAsync(Guid curriculumId);

    #endregion

    #region SchoolYears

    Task<SchoolYearModel> GetCurrentSchoolYear();
    Task<SchoolYearModel> GetSchoolYearAsync(Guid id);
    Task<IList<SchoolYearModel>> GetSchoolYearsAsync(int from = 0, int to = 0);
    Task<int> GetSchoolYearTotalAsync();
    Task<bool> ExistSchoolYearAsync(Guid id);
    Task<bool> CreateSchoolYearAsync(SchoolYearModel schoolYear);
    Task<bool> UpdateSchoolYearAsync(SchoolYearModel schoolYear);
    Task<bool> DeleteSchoolYearAsync(Guid schoolYear);

    #endregion

    #region Courses

    Task<Guid> CreateCourseAsync(CourseModel course);
    Task<bool> ExistsCourseAsync(Guid id);
    Task<bool> CheckCourseExistenceByCareerYearAndModality(Guid careerId, int careerYear, int modality);
    Task<int> GetCoursesCountAsync();
    Task<IList<CourseModel>> GetCoursesAsync(int from = 0, int to = 0);
    Task<IList<CourseModel>> GetCoursesAsync(Guid schoolYearId);
    Task<CourseModel> GetCourseAsync(Guid id);
    Task<bool> UpdateCourseAsync(CourseModel course);
    Task<bool> DeleteCourseAsync(Guid id);

    Task<IList<CourseModel>> GetCoursesForDepartment(Guid departmentId, Guid? schoolYearId = null);

    #endregion

    #region Periods

    Task<bool> CreatePeriodAsync(PeriodModel period);
    Task<bool> ExistsPeriodAsync(Guid id);
    Task<int> GetPeriodsCountAsync();
    Task<IList<PeriodModel>> GetPeriodsAsync(int from, int to);
    Task<IList<PeriodModel>> GetPeriodsAsync(Guid? schoolYearId = null);
    Task<PeriodModel> GetPeriodAsync(Guid id);
    Task<bool> UpdatePeriodAsync(PeriodModel period);
    Task<bool> DeletePeriodAsync(Guid id);

    #endregion

    #region TeachingPlanItems

    Task<bool> CreateTeachingPlanItemAsync(TeachingPlanItemModel item);
    Task<bool> ExistsTeachingPlanItemAsync(Guid id);
    Task<int> GetTeachingPlanItemsCountAsync();
    Task<int> GetTeachingPlanItemsCountAsync(Guid periodId);
    Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(int from = 0, int to = 0);
    Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(Guid periodId, int from = 0, int to = 0);
    Task<TeachingPlanItemModel> GetTeachingPlanItemAsync(Guid id);
    Task<bool> UpdateTeachingPlanItemAsync(TeachingPlanItemModel period);
    Task<bool> DeleteTeachingPlanItemAsync(Guid id);

    Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsOfDepartmentOnPeriodAsync(Guid departmentId, Guid periodId, Guid? courseId = null);

    #endregion

    #region Statistics

    Task<IList<StatisticItemModel>> GetGlobalStatisticsAsync();
    Task<IList<StatisticItemModel>> GetGlobalStatisticsForDepartmentAsync(Guid departmentId);

    #endregion
}