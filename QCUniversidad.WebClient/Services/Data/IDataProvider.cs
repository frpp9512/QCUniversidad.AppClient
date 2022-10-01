using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Curriculums;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.SchoolYears;
using QCUniversidad.WebClient.Models.Subjects;
using QCUniversidad.WebClient.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    Task<IList<TeacherModel>> GetTeachersAsync(int from, int to);
    Task<int> GetTeachersCountAsync();
    Task<bool> ExistsTeacherAsync(Guid id);
    Task<TeacherModel> GetTeacherAsync(Guid teacherId);
    Task<bool> CreateTeacherAsync(TeacherModel newTeacher);
    Task<bool> UpdateTeacherAsync(TeacherModel teacher);
    Task<bool> DeleteTeacherAsync(Guid teacherId);

    #endregion

    #region Subjects

    Task<IList<SubjectModel>> GetSubjectsAsync(int from, int to);
    Task<int> GetSubjectsCountAsync();
    Task<bool> ExistsSubjectAsync(Guid id);
    Task<SubjectModel> GetSubjectAsync(Guid subjectId);
    Task<bool> CreateSubjectAsync(SubjectModel newSubject);
    Task<bool> UpdateSubjectAsync(SubjectModel subject);
    Task<bool> DeleteSubjectAsync(Guid subjectId);

    #endregion

    #region Curriculums

    Task<IList<CurriculumModel>> GetCurriculumsAsync(int from = 0, int to = 0);
    Task<int> GetCurriculumsCountAsync();
    Task<bool> ExistsCurriculumAsync(Guid id);
    Task<CurriculumModel> GetCurriculumAsync(Guid curriculumId);
    Task<bool> CreateCurriculumAsync(CurriculumModel newCurriculum);
    Task<bool> UpdateCurriculumAsync(CurriculumModel curriculum);
    Task<bool> DeleteCurriculumAsync(Guid curriculumId);

    #endregion

    #region SchoolYears

    Task<bool> CreateSchoolYearAsync(SchoolYearModel schoolYear);
    Task<bool> ExistsSchoolYearAsync(Guid id);
    Task<int> GetSchoolYearsCountAsync();
    Task<int> GetSchoolYearPeriodsCountAsync(Guid schoolYearId);
    Task<IList<SchoolYearModel>> GetSchoolYearsAsync(int from, int to);
    Task<SchoolYearModel> GetSchoolYearAsync(Guid id);
    Task<bool> UpdateSchoolYearAsync(SchoolYearModel schoolYear);
    Task<bool> DeleteSchoolYearAsync(Guid id);

    #endregion

    #region Periods

    Task<bool> CreatePeriodAsync(PeriodModel period);
    Task<bool> ExistsPeriodAsync(Guid id);
    Task<int> GetPeriodsCountAsync();
    Task<IList<PeriodModel>> GetPeriodsAsync(int from, int to);
    Task<PeriodModel> GetPeriodAsync(Guid id);
    Task<bool> UpdatePeriodAsync(PeriodModel period);
    Task<bool> DeletePeriodAsync(Guid id);

    #endregion
}