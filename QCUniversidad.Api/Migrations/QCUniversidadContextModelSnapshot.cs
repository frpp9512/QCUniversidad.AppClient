﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QCUniversidad.Api.Data.Context;

#nullable disable

namespace QCUniversidad.Api.Migrations
{
    [DbContext(typeof(QCUniversidadContext))]
    partial class QCUniversidadContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.CareerModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("PostgraduateCourse")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Careers");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.CourseModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CareerId")
                        .HasColumnType("TEXT");

                    b.Property<int>("CareerYear")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("CurriculumId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Denomination")
                        .HasColumnType("TEXT");

                    b.Property<uint>("Enrolment")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LastCourse")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("SchoolYearId")
                        .HasColumnType("TEXT");

                    b.Property<int>("TeachingModality")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CareerId");

                    b.HasIndex("CurriculumId");

                    b.HasIndex("SchoolYearId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.CurriculumDiscipline", b =>
                {
                    b.Property<Guid>("CurriculumId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DisciplineId")
                        .HasColumnType("TEXT");

                    b.HasKey("CurriculumId", "DisciplineId");

                    b.HasIndex("DisciplineId");

                    b.ToTable("CurriculumsDisciplines");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.CurriculumModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CareerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Denomination")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CareerId");

                    b.ToTable("Curriculums");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.DepartmentModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("TEXT");

                    b.Property<string>("InternalId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.DisciplineModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Disciplines");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.FacultyModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Campus")
                        .HasColumnType("TEXT");

                    b.Property<string>("InternalId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.LoadItemModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<double>("HoursCovered")
                        .HasColumnType("REAL");

                    b.Property<Guid>("PlanningItemId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PlanningItemId");

                    b.HasIndex("TeacherId");

                    b.ToTable("LoadItems");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.NonTeachingLoadModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("BaseValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<double>("Load")
                        .HasColumnType("REAL");

                    b.Property<Guid>("PeriodId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PeriodId");

                    b.HasIndex("TeacherId");

                    b.ToTable("NonTeachingLoad");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.PeriodModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Ends")
                        .HasColumnType("TEXT");

                    b.Property<double>("MonthsCount")
                        .HasColumnType("REAL");

                    b.Property<Guid>("SchoolYearId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Starts")
                        .HasColumnType("TEXT");

                    b.Property<double>("TimeFund")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("SchoolYearId");

                    b.ToTable("Periods");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.PeriodSubjectModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("HaveFinalExam")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MidtermExamsCount")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PeriodId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("PeriodId");

                    b.HasIndex("SubjectId");

                    b.ToTable("PeriodSubjects");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.SchoolYearModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Current")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SchoolYears");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.SubjectModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DisciplineId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DisciplineId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.TeacherDiscipline", b =>
                {
                    b.Property<Guid>("TeacherId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DisciplineId")
                        .HasColumnType("TEXT");

                    b.HasKey("TeacherId", "DisciplineId");

                    b.HasIndex("DisciplineId");

                    b.ToTable("TeachersDisciplines");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.TeacherModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Category")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ContractType")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Fullname")
                        .HasColumnType("TEXT");

                    b.Property<string>("PersonalId")
                        .HasMaxLength(11)
                        .HasColumnType("TEXT");

                    b.Property<string>("Position")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.TeachingPlanItemModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("FromPostgraduateCourse")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("GroupsAmount")
                        .HasColumnType("INTEGER");

                    b.Property<double>("HoursPlanned")
                        .HasColumnType("REAL");

                    b.Property<Guid>("PeriodId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("PeriodId");

                    b.HasIndex("SubjectId");

                    b.ToTable("TeachingPlanItems");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.CareerModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.FacultyModel", "Faculty")
                        .WithMany("Carreers")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.CourseModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.CareerModel", "Career")
                        .WithMany("Courses")
                        .HasForeignKey("CareerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QCUniversidad.Api.Data.Models.CurriculumModel", "Curriculum")
                        .WithMany()
                        .HasForeignKey("CurriculumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QCUniversidad.Api.Data.Models.SchoolYearModel", "SchoolYear")
                        .WithMany("Courses")
                        .HasForeignKey("SchoolYearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Career");

                    b.Navigation("Curriculum");

                    b.Navigation("SchoolYear");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.CurriculumDiscipline", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.CurriculumModel", "Curriculum")
                        .WithMany("CurriculumDisciplines")
                        .HasForeignKey("CurriculumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QCUniversidad.Api.Data.Models.DisciplineModel", "Discipline")
                        .WithMany("DisciplineCurriculums")
                        .HasForeignKey("DisciplineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Curriculum");

                    b.Navigation("Discipline");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.CurriculumModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.CareerModel", "Career")
                        .WithMany("Curricula")
                        .HasForeignKey("CareerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Career");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.DepartmentModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.FacultyModel", "Faculty")
                        .WithMany("Deparments")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.DisciplineModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.DepartmentModel", "Department")
                        .WithMany("Disciplines")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.LoadItemModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.TeachingPlanItemModel", "PlanningItem")
                        .WithMany("LoadItems")
                        .HasForeignKey("PlanningItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QCUniversidad.Api.Data.Models.TeacherModel", "Teacher")
                        .WithMany("LoadItems")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlanningItem");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.NonTeachingLoadModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.PeriodModel", "Period")
                        .WithMany("NonTeachingLoad")
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QCUniversidad.Api.Data.Models.TeacherModel", "Teacher")
                        .WithMany("NonTeachingLoadItems")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Period");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.PeriodModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.SchoolYearModel", "SchoolYear")
                        .WithMany("Periods")
                        .HasForeignKey("SchoolYearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SchoolYear");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.PeriodSubjectModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.CourseModel", "Course")
                        .WithMany("PeriodSubjects")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QCUniversidad.Api.Data.Models.PeriodModel", "Period")
                        .WithMany("PeriodSubjects")
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QCUniversidad.Api.Data.Models.SubjectModel", "Subject")
                        .WithMany("PeriodsSubject")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Period");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.SubjectModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.DisciplineModel", "Discipline")
                        .WithMany("Subjects")
                        .HasForeignKey("DisciplineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discipline");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.TeacherDiscipline", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.DisciplineModel", "Discipline")
                        .WithMany("DisciplineTeachers")
                        .HasForeignKey("DisciplineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QCUniversidad.Api.Data.Models.TeacherModel", "Teacher")
                        .WithMany("TeacherDisciplines")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discipline");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.TeacherModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.DepartmentModel", "Department")
                        .WithMany("Teachers")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.TeachingPlanItemModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.CourseModel", "Course")
                        .WithMany("PlanItems")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QCUniversidad.Api.Data.Models.PeriodModel", "Period")
                        .WithMany("TeachingPlan")
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QCUniversidad.Api.Data.Models.SubjectModel", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Period");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.CareerModel", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Curricula");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.CourseModel", b =>
                {
                    b.Navigation("PeriodSubjects");

                    b.Navigation("PlanItems");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.CurriculumModel", b =>
                {
                    b.Navigation("CurriculumDisciplines");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.DepartmentModel", b =>
                {
                    b.Navigation("Disciplines");

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.DisciplineModel", b =>
                {
                    b.Navigation("DisciplineCurriculums");

                    b.Navigation("DisciplineTeachers");

                    b.Navigation("Subjects");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.FacultyModel", b =>
                {
                    b.Navigation("Carreers");

                    b.Navigation("Deparments");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.PeriodModel", b =>
                {
                    b.Navigation("NonTeachingLoad");

                    b.Navigation("PeriodSubjects");

                    b.Navigation("TeachingPlan");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.SchoolYearModel", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Periods");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.SubjectModel", b =>
                {
                    b.Navigation("PeriodsSubject");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.TeacherModel", b =>
                {
                    b.Navigation("LoadItems");

                    b.Navigation("NonTeachingLoadItems");

                    b.Navigation("TeacherDisciplines");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.TeachingPlanItemModel", b =>
                {
                    b.Navigation("LoadItems");
                });
#pragma warning restore 612, 618
        }
    }
}
