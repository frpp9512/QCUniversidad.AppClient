﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QCUniversidad.Api.Data.Context;

#nullable disable

namespace QCUniversidad.Api.Migrations
{
    [DbContext(typeof(QCUniversidadContext))]
    [Migration("20221009213102_RemoveTeachingPlanModel")]
    partial class RemoveTeachingPlanModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Careers");
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

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.PeriodModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Ends")
                        .HasColumnType("TEXT");

                    b.Property<uint>("Enrolment")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderNumber")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("SchoolYearId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Starts")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SchoolYearId");

                    b.ToTable("Periods");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.SchoolYearModel", b =>
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
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Ends")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Starts")
                        .HasColumnType("TEXT");

                    b.Property<int>("TeachingModality")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CareerId");

                    b.HasIndex("CurriculumId");

                    b.ToTable("SchoolYears");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.SubjectModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

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

                    b.Property<int>("Category")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Fullname")
                        .IsRequired()
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

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.PeriodModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.SchoolYearModel", "SchoolYear")
                        .WithMany("Periods")
                        .HasForeignKey("SchoolYearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SchoolYear");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.SchoolYearModel", b =>
                {
                    b.HasOne("QCUniversidad.Api.Data.Models.CareerModel", "Career")
                        .WithMany("SchoolYears")
                        .HasForeignKey("CareerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QCUniversidad.Api.Data.Models.CurriculumModel", "Curriculum")
                        .WithMany()
                        .HasForeignKey("CurriculumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Career");

                    b.Navigation("Curriculum");
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

                    b.Navigation("Period");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.CareerModel", b =>
                {
                    b.Navigation("Curricula");

                    b.Navigation("SchoolYears");
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
                    b.Navigation("TeachingPlan");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.SchoolYearModel", b =>
                {
                    b.Navigation("Periods");
                });

            modelBuilder.Entity("QCUniversidad.Api.Data.Models.TeacherModel", b =>
                {
                    b.Navigation("TeacherDisciplines");
                });
#pragma warning restore 612, 618
        }
    }
}
