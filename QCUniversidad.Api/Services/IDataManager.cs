﻿using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Services
{
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

        #endregion

        #region Subjects

        Task<bool> CreateSubjectAsync(SubjectModel subject);
        Task<bool> ExistsSubjectAsync(Guid id);
        Task<int> GetSubjectsCountAsync();
        Task<int> GetSubjectCurriculumsCountAsync(Guid id);
        Task<IList<SubjectModel>> GetSubjectsAsync(int from, int to);
        Task<SubjectModel> GetSubjectAsync(Guid id);
        Task<bool> UpdateSubjectAsync(SubjectModel subject);
        Task<bool> DeleteSubjectAsync(Guid id);

        #endregion

        #region Teachers - Disciplines

        Task<IList<DisciplineModel>> GetDisciplinesForTeacher(Guid teacherId);

        #endregion
    }
}