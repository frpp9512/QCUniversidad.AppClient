using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations;

public partial class initnpgsql : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "Faculties",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Campus = table.Column<string>(type: "text", nullable: true),
                InternalId = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Faculties", x => x.Id));

        _ = migrationBuilder.CreateTable(
            name: "SchoolYears",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                Current = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_SchoolYears", x => x.Id));

        _ = migrationBuilder.CreateTable(
            name: "Careers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                PostgraduateCourse = table.Column<bool>(type: "boolean", nullable: false),
                FacultyId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Careers", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_Careers_Faculties_FacultyId",
                    column: x => x.FacultyId,
                    principalTable: "Faculties",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "Departments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                InternalId = table.Column<string>(type: "text", nullable: false),
                FacultyId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Departments", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_Departments_Faculties_FacultyId",
                    column: x => x.FacultyId,
                    principalTable: "Faculties",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "Periods",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                Starts = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                Ends = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                MonthsCount = table.Column<double>(type: "double precision", nullable: false),
                TimeFund = table.Column<double>(type: "double precision", nullable: false),
                SchoolYearId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Periods", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_Periods_SchoolYears_SchoolYearId",
                    column: x => x.SchoolYearId,
                    principalTable: "SchoolYears",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "Curriculums",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Denomination = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                CareerId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Curriculums", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_Curriculums_Careers_CareerId",
                    column: x => x.CareerId,
                    principalTable: "Careers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "DepartmentsCareers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                CareerId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_DepartmentsCareers", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_DepartmentsCareers_Careers_CareerId",
                    column: x => x.CareerId,
                    principalTable: "Careers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_DepartmentsCareers_Departments_DepartmentId",
                    column: x => x.DepartmentId,
                    principalTable: "Departments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "Disciplines",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                DepartmentId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Disciplines", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_Disciplines_Departments_DepartmentId",
                    column: x => x.DepartmentId,
                    principalTable: "Departments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "Teachers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Fullname = table.Column<string>(type: "text", nullable: true),
                PersonalId = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                Position = table.Column<string>(type: "text", nullable: true),
                Category = table.Column<int>(type: "integer", nullable: false),
                ContractType = table.Column<int>(type: "integer", nullable: false),
                Email = table.Column<string>(type: "text", nullable: true),
                ServiceProvider = table.Column<bool>(type: "boolean", nullable: false),
                DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                Active = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Teachers", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_Teachers_Departments_DepartmentId",
                    column: x => x.DepartmentId,
                    principalTable: "Departments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "Courses",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SchoolYearId = table.Column<Guid>(type: "uuid", nullable: false),
                CareerYear = table.Column<int>(type: "integer", nullable: false),
                LastCourse = table.Column<bool>(type: "boolean", nullable: false),
                Denomination = table.Column<string>(type: "text", nullable: true),
                TeachingModality = table.Column<int>(type: "integer", nullable: false),
                Enrolment = table.Column<long>(type: "bigint", nullable: false),
                CareerId = table.Column<Guid>(type: "uuid", nullable: false),
                CurriculumId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Courses", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_Courses_Careers_CareerId",
                    column: x => x.CareerId,
                    principalTable: "Careers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_Courses_Curriculums_CurriculumId",
                    column: x => x.CurriculumId,
                    principalTable: "Curriculums",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_Courses_SchoolYears_SchoolYearId",
                    column: x => x.SchoolYearId,
                    principalTable: "SchoolYears",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "CurriculumsDisciplines",
            columns: table => new
            {
                CurriculumId = table.Column<Guid>(type: "uuid", nullable: false),
                DisciplineId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_CurriculumsDisciplines", x => new { x.CurriculumId, x.DisciplineId });
                _ = table.ForeignKey(
                    name: "FK_CurriculumsDisciplines_Curriculums_CurriculumId",
                    column: x => x.CurriculumId,
                    principalTable: "Curriculums",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_CurriculumsDisciplines_Disciplines_DisciplineId",
                    column: x => x.DisciplineId,
                    principalTable: "Disciplines",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "Subjects",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                DisciplineId = table.Column<Guid>(type: "uuid", nullable: false),
                Active = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Subjects", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_Subjects_Disciplines_DisciplineId",
                    column: x => x.DisciplineId,
                    principalTable: "Disciplines",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "NonTeachingLoad",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                PeriodId = table.Column<Guid>(type: "uuid", nullable: false),
                TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                Type = table.Column<int>(type: "integer", nullable: false),
                BaseValue = table.Column<string>(type: "text", nullable: true),
                Load = table.Column<double>(type: "double precision", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_NonTeachingLoad", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_NonTeachingLoad_Periods_PeriodId",
                    column: x => x.PeriodId,
                    principalTable: "Periods",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_NonTeachingLoad_Teachers_TeacherId",
                    column: x => x.TeacherId,
                    principalTable: "Teachers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "TeachersDisciplines",
            columns: table => new
            {
                TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                DisciplineId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_TeachersDisciplines", x => new { x.TeacherId, x.DisciplineId });
                _ = table.ForeignKey(
                    name: "FK_TeachersDisciplines_Disciplines_DisciplineId",
                    column: x => x.DisciplineId,
                    principalTable: "Disciplines",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_TeachersDisciplines_Teachers_TeacherId",
                    column: x => x.TeacherId,
                    principalTable: "Teachers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "PeriodSubjects",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                PeriodId = table.Column<Guid>(type: "uuid", nullable: false),
                SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                MidtermExamsCount = table.Column<int>(type: "integer", nullable: false),
                HaveFinalExam = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_PeriodSubjects", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_PeriodSubjects_Courses_CourseId",
                    column: x => x.CourseId,
                    principalTable: "Courses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_PeriodSubjects_Periods_PeriodId",
                    column: x => x.PeriodId,
                    principalTable: "Periods",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_PeriodSubjects_Subjects_SubjectId",
                    column: x => x.SubjectId,
                    principalTable: "Subjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "TeachingPlanItems",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                Type = table.Column<int>(type: "integer", nullable: false),
                HoursPlanned = table.Column<double>(type: "double precision", nullable: false),
                GroupsAmount = table.Column<long>(type: "bigint", nullable: false),
                FromPostgraduateCourse = table.Column<bool>(type: "boolean", nullable: false),
                PeriodId = table.Column<Guid>(type: "uuid", nullable: false),
                CourseId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_TeachingPlanItems", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_TeachingPlanItems_Courses_CourseId",
                    column: x => x.CourseId,
                    principalTable: "Courses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_TeachingPlanItems_Periods_PeriodId",
                    column: x => x.PeriodId,
                    principalTable: "Periods",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_TeachingPlanItems_Subjects_SubjectId",
                    column: x => x.SubjectId,
                    principalTable: "Subjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "LoadItems",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                PlanningItemId = table.Column<Guid>(type: "uuid", nullable: false),
                TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                HoursCovered = table.Column<double>(type: "double precision", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_LoadItems", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_LoadItems_Teachers_TeacherId",
                    column: x => x.TeacherId,
                    principalTable: "Teachers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_LoadItems_TeachingPlanItems_PlanningItemId",
                    column: x => x.PlanningItemId,
                    principalTable: "TeachingPlanItems",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_Careers_FacultyId",
            table: "Careers",
            column: "FacultyId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Courses_CareerId",
            table: "Courses",
            column: "CareerId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Courses_CurriculumId",
            table: "Courses",
            column: "CurriculumId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Courses_SchoolYearId",
            table: "Courses",
            column: "SchoolYearId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Curriculums_CareerId",
            table: "Curriculums",
            column: "CareerId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_CurriculumsDisciplines_DisciplineId",
            table: "CurriculumsDisciplines",
            column: "DisciplineId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Departments_FacultyId",
            table: "Departments",
            column: "FacultyId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_DepartmentsCareers_CareerId",
            table: "DepartmentsCareers",
            column: "CareerId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_DepartmentsCareers_DepartmentId",
            table: "DepartmentsCareers",
            column: "DepartmentId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Disciplines_DepartmentId",
            table: "Disciplines",
            column: "DepartmentId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_LoadItems_PlanningItemId",
            table: "LoadItems",
            column: "PlanningItemId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_LoadItems_TeacherId",
            table: "LoadItems",
            column: "TeacherId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_NonTeachingLoad_PeriodId",
            table: "NonTeachingLoad",
            column: "PeriodId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_NonTeachingLoad_TeacherId",
            table: "NonTeachingLoad",
            column: "TeacherId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Periods_SchoolYearId",
            table: "Periods",
            column: "SchoolYearId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_PeriodSubjects_CourseId",
            table: "PeriodSubjects",
            column: "CourseId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_PeriodSubjects_PeriodId",
            table: "PeriodSubjects",
            column: "PeriodId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_PeriodSubjects_SubjectId",
            table: "PeriodSubjects",
            column: "SubjectId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Subjects_DisciplineId",
            table: "Subjects",
            column: "DisciplineId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Teachers_DepartmentId",
            table: "Teachers",
            column: "DepartmentId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_TeachersDisciplines_DisciplineId",
            table: "TeachersDisciplines",
            column: "DisciplineId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_TeachingPlanItems_CourseId",
            table: "TeachingPlanItems",
            column: "CourseId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_TeachingPlanItems_PeriodId",
            table: "TeachingPlanItems",
            column: "PeriodId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_TeachingPlanItems_SubjectId",
            table: "TeachingPlanItems",
            column: "SubjectId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "CurriculumsDisciplines");

        _ = migrationBuilder.DropTable(
            name: "DepartmentsCareers");

        _ = migrationBuilder.DropTable(
            name: "LoadItems");

        _ = migrationBuilder.DropTable(
            name: "NonTeachingLoad");

        _ = migrationBuilder.DropTable(
            name: "PeriodSubjects");

        _ = migrationBuilder.DropTable(
            name: "TeachersDisciplines");

        _ = migrationBuilder.DropTable(
            name: "TeachingPlanItems");

        _ = migrationBuilder.DropTable(
            name: "Teachers");

        _ = migrationBuilder.DropTable(
            name: "Courses");

        _ = migrationBuilder.DropTable(
            name: "Periods");

        _ = migrationBuilder.DropTable(
            name: "Subjects");

        _ = migrationBuilder.DropTable(
            name: "Curriculums");

        _ = migrationBuilder.DropTable(
            name: "SchoolYears");

        _ = migrationBuilder.DropTable(
            name: "Disciplines");

        _ = migrationBuilder.DropTable(
            name: "Careers");

        _ = migrationBuilder.DropTable(
            name: "Departments");

        _ = migrationBuilder.DropTable(
            name: "Faculties");
    }
}
