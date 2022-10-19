using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Data.Context
{
    public class QCUniversidadContext : DbContext
    {
        public DbSet<FacultyModel> Faculties { get; set; }
        public DbSet<CareerModel> Careers { get; set; }

        public DbSet<CurriculumModel> Curriculums { get; set; }
        public DbSet<SubjectModel> Subjects { get; set; }
        public DbSet<CurriculumDiscipline> CurriculumsDisciplines { get; set; }

        public DbSet<SchoolYearModel> SchoolYears { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<PeriodModel> Periods { get; set; }

        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<DisciplineModel> Disciplines { get; set; }
        public DbSet<TeacherModel> Teachers { get; set; }
        public DbSet<TeacherDiscipline> TeachersDisciplines { get; set; }

        public DbSet<TeachingPlanItemModel> TeachingPlanItems { get; set; }

        public DbSet<LoadItemModel> LoadItems { get; set; }

        public QCUniversidadContext(DbContextOptions<QCUniversidadContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FacultyModel>()
                        .HasMany(f => f.Deparments)
                        .WithOne(d => d.Faculty)
                        .HasForeignKey(d => d.FacultyId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CareerModel>()
                        .HasOne(c => c.Faculty)
                        .WithMany(f => f.Carreers)
                        .HasForeignKey(c => c.FacultyId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SchoolYearModel>()
                        .HasMany(sy => sy.Courses)
                        .WithOne(c => c.SchoolYear)
                        .HasForeignKey(c => c.SchoolYearId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseModel>()
                        .HasMany(c => c.Periods)
                        .WithOne(p => p.Course)
                        .HasForeignKey(c => c.CourseId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseModel>()
                        .HasOne(c => c.Career)
                        .WithMany(cr => cr.Courses)
                        .HasForeignKey(c => c.CareerId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DisciplineModel>()
                        .HasMany(d => d.DisciplineTeachers)
                        .WithOne(dt => dt.Discipline)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubjectModel>()
                        .HasOne(s => s.Discipline)
                        .WithMany(d => d.Subjects)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeacherModel>()
                        .HasOne(t => t.Department)
                        .WithMany(d => d.Teachers)
                        .HasForeignKey(t => t.DepartmentId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeacherDiscipline>()
                        .HasKey(td => new { td.TeacherId, td.DisciplineId });

            modelBuilder.Entity<TeacherDiscipline>()
                        .HasOne(td => td.Teacher)
                        .WithMany(t => t.TeacherDisciplines)
                        .HasForeignKey(td => td.TeacherId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeacherDiscipline>()
                        .HasOne(td => td.Discipline)
                        .WithMany(d => d.DisciplineTeachers)
                        .HasForeignKey(td => td.DisciplineId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CurriculumModel>()
                        .HasOne(c => c.Career)
                        .WithMany(c => c.Curricula)
                        .HasForeignKey(c => c.CareerId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CurriculumDiscipline>()
                        .HasKey(cs => new { cs.CurriculumId, cs.DisciplineId });

            modelBuilder.Entity<CurriculumDiscipline>()
                        .HasOne(cs => cs.Curriculum)
                        .WithMany(c => c.CurriculumDisciplines)
                        .HasForeignKey(cs => cs.CurriculumId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CurriculumDiscipline>()
                        .HasOne(cs => cs.Discipline)
                        .WithMany(s => s.DisciplineCurriculums)
                        .HasForeignKey(cs => cs.DisciplineId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DepartmentModel>()
                        .HasMany(d => d.Disciplines)
                        .WithOne(d => d.Department)
                        .HasForeignKey(d => d.DepartmentId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeachingPlanItemModel>()
                        .HasOne(tp => tp.Period)
                        .WithMany(p => p.TeachingPlan)
                        .HasForeignKey(tp => tp.PeriodId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LoadItemModel>()
                        .HasOne(li => li.PlanningItem)
                        .WithMany(pi => pi.LoadItems)
                        .HasForeignKey(li => li.PlanningItemId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LoadItemModel>()
                        .HasOne(li => li.Teacher)
                        .WithMany(t => t.LoadItems)
                        .HasForeignKey(li => li.TeacherId)
                        .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}