using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public DbSet<CurriculumSubject> CurriculumsSubjects { get; set; }

        public DbSet<SchoolYearModel> SchoolYears { get; set; }
        public DbSet<PeriodModel> Periods { get; set; }

        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<DisciplineModel> Disciplines { get; set; }
        public DbSet<TeacherModel> Teachers { get; set; }
        public DbSet<TeacherDiscipline> TeachersDisciplines { get; set; }

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
                        .HasMany(sy => sy.Periods)
                        .WithOne(d => d.SchoolYear)
                        .HasForeignKey(sy => sy.ShoolYearId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DisciplineModel>()
                        .HasMany(d => d.DisciplineTeachers)
                        .WithOne(d => d.Discipline)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubjectModel>()
                        .HasOne(s => s.Discipline)
                        .WithMany(d => d.Subjects)
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

            modelBuilder.Entity<CurriculumSubject>()
                        .HasKey(cs => new { cs.CurriculumId, cs.SubjectId });

            modelBuilder.Entity<CurriculumSubject>()
                        .HasOne(cs => cs.Curriculum)
                        .WithMany(c => c.CurriculumSubjects)
                        .HasForeignKey(cs => cs.CurriculumId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CurriculumSubject>()
                        .HasOne(cs => cs.Subject)
                        .WithMany(s => s.SubjectCurriculums)
                        .HasForeignKey(cs => cs.SubjectId)
                        .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}