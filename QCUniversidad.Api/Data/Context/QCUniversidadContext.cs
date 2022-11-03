using Microsoft.EntityFrameworkCore;
using QCUniversidad.Api.Data.Models;

namespace QCUniversidad.Api.Data.Context;

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
    public DbSet<DepartmentCareer> DepartmentsCareers { get; set; }
    public DbSet<DisciplineModel> Disciplines { get; set; }
    public DbSet<TeacherModel> Teachers { get; set; }
    public DbSet<TeacherDiscipline> TeachersDisciplines { get; set; }

    public DbSet<PeriodSubjectModel> PeriodSubjects { get; set; }
    public DbSet<TeachingPlanItemModel> TeachingPlanItems { get; set; }

    public DbSet<LoadItemModel> LoadItems { get; set; }
    public DbSet<NonTeachingLoadModel> NonTeachingLoad { get; set; }

    public QCUniversidadContext(DbContextOptions<QCUniversidadContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<FacultyModel>()
                    .HasMany(f => f.Deparments)
                    .WithOne(d => d.Faculty)
                    .HasForeignKey(d => d.FacultyId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<CareerModel>()
                    .HasOne(c => c.Faculty)
                    .WithMany(f => f.Carreers)
                    .HasForeignKey(c => c.FacultyId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<SchoolYearModel>()
                    .HasMany(sy => sy.Courses)
                    .WithOne(c => c.SchoolYear)
                    .HasForeignKey(c => c.SchoolYearId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<SchoolYearModel>()
                    .HasMany(sy => sy.Periods)
                    .WithOne(p => p.SchoolYear)
                    .HasForeignKey(p => p.SchoolYearId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<CourseModel>()
                    .HasOne(c => c.Career)
                    .WithMany(cr => cr.Courses)
                    .HasForeignKey(c => c.CareerId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<CourseModel>()
                    .HasMany(c => c.PlanItems)
                    .WithOne(i => i.Course)
                    .HasForeignKey(i => i.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<DisciplineModel>()
                    .HasMany(d => d.DisciplineTeachers)
                    .WithOne(dt => dt.Discipline)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<SubjectModel>()
                    .HasOne(s => s.Discipline)
                    .WithMany(d => d.Subjects)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<TeacherModel>()
                    .HasOne(t => t.Department)
                    .WithMany(d => d.Teachers)
                    .HasForeignKey(t => t.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<TeacherDiscipline>()
                    .HasKey(td => new { td.TeacherId, td.DisciplineId });

        _ = modelBuilder.Entity<TeacherDiscipline>()
                    .HasOne(td => td.Teacher)
                    .WithMany(t => t.TeacherDisciplines)
                    .HasForeignKey(td => td.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<TeacherDiscipline>()
                    .HasOne(td => td.Discipline)
                    .WithMany(d => d.DisciplineTeachers)
                    .HasForeignKey(td => td.DisciplineId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<CurriculumModel>()
                    .HasOne(c => c.Career)
                    .WithMany(c => c.Curricula)
                    .HasForeignKey(c => c.CareerId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<CurriculumDiscipline>()
                    .HasKey(cs => new { cs.CurriculumId, cs.DisciplineId });

        _ = modelBuilder.Entity<CurriculumDiscipline>()
                    .HasOne(cs => cs.Curriculum)
                    .WithMany(c => c.CurriculumDisciplines)
                    .HasForeignKey(cs => cs.CurriculumId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<CurriculumDiscipline>()
                    .HasOne(cs => cs.Discipline)
                    .WithMany(s => s.DisciplineCurriculums)
                    .HasForeignKey(cs => cs.DisciplineId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<DepartmentModel>()
                        .HasMany(d => d.Disciplines)
                        .WithOne(d => d.Department)
                        .HasForeignKey(d => d.DepartmentId)
                        .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<DepartmentCareer>()
                        .HasOne(dc => dc.Department)
                        .WithMany(d => d.DepartmentCareers)
                        .HasForeignKey(dc => dc.DepartmentId)
                        .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<DepartmentCareer>()
                        .HasOne(dc => dc.Career)
                        .WithMany(c => c.CareerDepartments)
                        .HasForeignKey(dc => dc.CareerId)
                        .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<PeriodSubjectModel>()
                    .HasOne(ps => ps.Period)
                    .WithMany(p => p.PeriodSubjects)
                    .HasForeignKey(ps => ps.PeriodId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<PeriodSubjectModel>()
                    .HasOne(ps => ps.Course)
                    .WithMany(c => c.PeriodSubjects)
                    .HasForeignKey(ps => ps.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<PeriodSubjectModel>()
                    .HasOne(ps => ps.Subject)
                    .WithMany(c => c.PeriodsSubject)
                    .HasForeignKey(ps => ps.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<TeachingPlanItemModel>()
                    .HasOne(tp => tp.Period)
                    .WithMany(p => p.TeachingPlan)
                    .HasForeignKey(tp => tp.PeriodId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<LoadItemModel>()
                    .HasOne(li => li.PlanningItem)
                    .WithMany(pi => pi.LoadItems)
                    .HasForeignKey(li => li.PlanningItemId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<LoadItemModel>()
                    .HasOne(li => li.Teacher)
                    .WithMany(t => t.LoadItems)
                    .HasForeignKey(li => li.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<NonTeachingLoadModel>()
                    .HasOne(l => l.Teacher)
                    .WithMany(t => t.NonTeachingLoadItems)
                    .HasForeignKey(l => l.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<NonTeachingLoadModel>()
                    .HasOne(l => l.Period)
                    .WithMany(p => p.NonTeachingLoad)
                    .HasForeignKey(l => l.PeriodId)
                    .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}