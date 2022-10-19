﻿using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Services;

public interface IDataManager
{
    #region Faculties

    Task<FacultyModel> GetFacultyAsync(Guid id);
    Task<IList<FacultyModel>> GetFacultiesAsync(int from = 0, int to = 0);
    Task<int> GetFacultiesTotalAsync();
    Task<bool> ExistFacultyAsync(Guid id);
    Task<bool> CreateFacultyAsync(FacultyModel faculty);
    Task<bool> UpdateFacultyAsync(FacultyModel faculty);
    Task<int> GetFacultyDepartmentCountAsync(Guid facultyId);
    Task<int> GetFacultyCareerCountAsync(Guid facultyId);
    Task<bool> DeleteFacultyAsync(Guid facultyId);

    #endregion

    #region Deparments

    Task<IList<DepartmentModel>> GetDepartmentsAsync(int from = 0, int to = 0);
    Task<IList<DepartmentModel>> GetDepartmentsAsync(Guid facultyId);
    Task<bool> ExistDepartmentAsync(Guid id);
    Task<int> GetDepartmentsCountAsync();
    Task<int> GetDepartmentsCountAsync(Guid facultyId);
    Task<int> GetDepartmentDisciplinesCount(Guid departmentId);
    Task<DepartmentModel> GetDepartmentAsync(Guid departmentId);
    Task<int> GetDeparmentTeachersCountAsync(Guid departmentId);
    Task<bool> CreateDepartmentAsync(DepartmentModel department);
    Task<bool> UpdateDeparmentAsync(DepartmentModel department);
    Task<bool> DeleteDeparmentAsync(Guid deparmentId);

    #endregion

    #region Careers

    Task<bool> ExistsCareerAsync(Guid id);
    Task<bool> CreateCareerAsync(CareerModel career);
    Task<IList<CareerModel>> GetCareersAsync(int from = 0, int to = 0);
    Task<IList<CareerModel>> GetCareersAsync(Guid facultyId);
    Task<CareerModel> GetCareerAsync(Guid careerId);
    Task<int> GetCareersCountAsync();
    Task<bool> UpdateCareerAsync(CareerModel career);
    Task<bool> DeleteCareerAsync(Guid careerId);

    #endregion

    #region Disciplines

    Task<bool> CreateDisciplineAsync(DisciplineModel discipline);
    Task<bool> ExistsDisciplineAsync(Guid id);
    Task<int> GetDisciplinesCountAsync();
    Task<int> GetDisciplineSubjectsCountAsync(Guid disciplineId);
    Task<int> GetDisciplineTeachersCountAsync(Guid disciplineId);
    Task<IList<DisciplineModel>> GetDisciplinesAsync(int from, int to);
    Task<DisciplineModel> GetDisciplineAsync(Guid disciplineId);
    Task<bool> UpdateDisciplineAsync(DisciplineModel discipline);
    Task<bool> DeleteDisciplineAsync(Guid disciplineId);

    #endregion

    #region Teachers

    Task<bool> CreateTeacherAsync(TeacherModel teacher);
    Task<bool> ExistsTeacherAsync(Guid id);
    Task<int> GetTeachersCountAsync();
    Task<int> GetTeacherDisciplinesCountAsync(Guid id);
    Task<IList<TeacherModel>> GetTeachersAsync(int from, int to);
    Task<TeacherModel> GetTeacherAsync(Guid id);
    Task<bool> UpdateTeacherAsync(TeacherModel teacher);
    Task<bool> DeleteTeacherAsync(Guid id);
    Task<IList<TeacherModel>> GetTeachersOfDepartmentAsync(Guid departmentId);
    Task<IList<TeacherModel>> GetTeachersOfDepartmentNotAssignedToPlanItemAsync(Guid departmentId, Guid planItemId, Guid? disciplineId = null);

    Task<bool> SetLoadToTeacher(Guid teacherId, Guid planItemId, double hours);

    #endregion

    #region Subjects

    Task<bool> CreateSubjectAsync(SubjectModel subject);
    Task<bool> ExistsSubjectAsync(Guid id);
    Task<IList<SubjectModel>> GetSubjectsForCourseAsync(Guid courseId);
    Task<int> GetSubjectsCountAsync();
    Task<IList<SubjectModel>> GetSubjectsAsync(int from, int to);
    Task<SubjectModel> GetSubjectAsync(Guid id);
    Task<bool> UpdateSubjectAsync(SubjectModel subject);
    Task<bool> DeleteSubjectAsync(Guid id);

    #endregion

    #region Curriculum
    
    Task<bool> CreateCurriculumAsync(CurriculumModel curriculum);
    Task<bool> ExistsCurriculumAsync(Guid id);
    Task<int> GetCurriculumsCountAsync();
    Task<int> GetCurriculumDisciplinesCountAsync(Guid id);
    Task<IList<CurriculumModel>> GetCurriculumsAsync(int from, int to);
    Task<CurriculumModel> GetCurriculumAsync(Guid id);
    Task<bool> UpdateCurriculumAsync(CurriculumModel curriculum);
    Task<bool> DeleteCurriculumAsync(Guid id);

    #endregion

    #region SchoolYears

    Task<SchoolYearModel> GetCurrentSchoolYear();
    Task<SchoolYearModel> GetSchoolYearAsync(Guid id);
    Task<IList<SchoolYearModel>> GetSchoolYearsAsync(int from = 0, int to = 0);
    Task<int> GetSchoolYearTotalAsync();
    Task<bool> ExistSchoolYearAsync(Guid id);
    Task<bool> CreateSchoolYearAsync(SchoolYearModel schoolYear);
    Task<bool> UpdateSchoolYearAsync(SchoolYearModel schoolYear);
    Task<int> GetSchoolYearCoursesCountAsync(Guid schoolYear);
    Task<bool> DeleteSchoolYearAsync(Guid schoolYear);

    #endregion

    #region Courses

    Task<bool> CreateCourseAsync(CourseModel course);
    Task<bool> ExistsCourseAsync(Guid id);
    Task<bool> CheckCourseExistenceByCareerYearAndModality(Guid careerId, int careerYear, TeachingModality modality);
    Task<int> GetCoursesCountAsync();
    Task<int> GetCoursePeriodsCountAsync(Guid courseId);
    Task<IList<CourseModel>> GetCoursesAsync(int from, int to);
    Task<CourseModel> GetCourseAsync(Guid id);
    Task<bool> UpdateCourseAsync(CourseModel course);
    Task<bool> DeleteCourseAsync(Guid id);

    Task<IList<CourseModel>> GetCoursesForDepartment(Guid departmentId, Guid? schoolYearId = null);

    #endregion

    #region Periods

    Task<bool> CreatePeriodAsync(PeriodModel period);
    Task<bool> ExistsPeriodAsync(Guid id);
    Task<bool> ExistPeriodWithOrder(Guid courseId, int order);
    Task<int> GetPeriodsCountAsync();
    Task<IList<PeriodModel>> GetPeriodsAsync(int from, int to);
    Task<PeriodModel> GetPeriodAsync(Guid id);
    Task<bool> UpdatePeriodAsync(PeriodModel period);
    Task<bool> DeletePeriodAsync(Guid id);

    Task<IList<PeriodModel>> GetPeriodsOfCourseForDepartment(Guid courseId, Guid departmentId);

    #endregion

    #region TeachingPlanItems

    Task<bool> CreateTeachingPlanItemAsync(TeachingPlanItemModel item);
    Task<bool> ExistsTeachingPlanItemAsync(Guid id);
    Task<int> GetTeachingPlanItemsCountAsync();
    Task<int> GetTeachingPlanItemsCountAsync(Guid periodId);
    Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(int from, int to);
    Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(Guid periodId, int from, int to);
    Task<TeachingPlanItemModel> GetTeachingPlanItemAsync(Guid id);
    Task<bool> UpdateTeachingPlanItemAsync(TeachingPlanItemModel period);
    Task<bool> DeleteTeachingPlanItemAsync(Guid id);

    Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsOfDepartmentOnPeriod(Guid departmentId, Guid periodId);
    Task<bool> IsTeachingPlanFromPostgraduateCourse(Guid teachingPlanId);
    Task<double> GetPlanItemTotalCoveredAsync(Guid planItemId);

    #endregion

    #region Teachers - Disciplines

    Task<IList<DisciplineModel>> GetDisciplinesForTeacher(Guid teacherId);

    #endregion
}