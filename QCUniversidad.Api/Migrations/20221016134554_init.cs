using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Campus = table.Column<string>(type: "TEXT", nullable: true),
                    InternalId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Careers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    PostgraduateCourse = table.Column<bool>(type: "INTEGER", nullable: false),
                    FacultyId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Careers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Careers_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    InternalId = table.Column<string>(type: "TEXT", nullable: false),
                    FacultyId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Curriculums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Denomination = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CareerId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curriculums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Curriculums_Careers_CareerId",
                        column: x => x.CareerId,
                        principalTable: "Careers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Disciplines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disciplines_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Fullname = table.Column<string>(type: "TEXT", nullable: false),
                    PersonalId = table.Column<string>(type: "TEXT", maxLength: 11, nullable: true),
                    Position = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CareerYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Denomination = table.Column<string>(type: "TEXT", nullable: false),
                    Starts = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Ends = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    TeachingModality = table.Column<int>(type: "INTEGER", nullable: false),
                    CareerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurriculumId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Careers_CareerId",
                        column: x => x.CareerId,
                        principalTable: "Careers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Curriculums_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curriculums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurriculumsDisciplines",
                columns: table => new
                {
                    CurriculumId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DisciplineId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumsDisciplines", x => new { x.CurriculumId, x.DisciplineId });
                    table.ForeignKey(
                        name: "FK_CurriculumsDisciplines_Curriculums_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curriculums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurriculumsDisciplines_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DisciplineId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeachersDisciplines",
                columns: table => new
                {
                    TeacherId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DisciplineId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachersDisciplines", x => new { x.TeacherId, x.DisciplineId });
                    table.ForeignKey(
                        name: "FK_TeachersDisciplines_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeachersDisciplines_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Starts = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Ends = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Enrolment = table.Column<uint>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Periods_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeachingPlanItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    HoursPlanned = table.Column<double>(type: "REAL", nullable: false),
                    GroupsAmount = table.Column<uint>(type: "INTEGER", nullable: false),
                    PeriodId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachingPlanItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeachingPlanItems_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeachingPlanItems_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoadItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlanningItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeacherId = table.Column<Guid>(type: "TEXT", nullable: false),
                    HoursCovered = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadItems_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoadItems_TeachingPlanItems_PlanningItemId",
                        column: x => x.PlanningItemId,
                        principalTable: "TeachingPlanItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Careers_FacultyId",
                table: "Careers",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CareerId",
                table: "Courses",
                column: "CareerId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CurriculumId",
                table: "Courses",
                column: "CurriculumId");

            migrationBuilder.CreateIndex(
                name: "IX_Curriculums_CareerId",
                table: "Curriculums",
                column: "CareerId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumsDisciplines_DisciplineId",
                table: "CurriculumsDisciplines",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_FacultyId",
                table: "Departments",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Disciplines_DepartmentId",
                table: "Disciplines",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadItems_PlanningItemId",
                table: "LoadItems",
                column: "PlanningItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadItems_TeacherId",
                table: "LoadItems",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_CourseId",
                table: "Periods",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_DisciplineId",
                table: "Subjects",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DepartmentId",
                table: "Teachers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachersDisciplines_DisciplineId",
                table: "TeachersDisciplines",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingPlanItems_PeriodId",
                table: "TeachingPlanItems",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingPlanItems_SubjectId",
                table: "TeachingPlanItems",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurriculumsDisciplines");

            migrationBuilder.DropTable(
                name: "LoadItems");

            migrationBuilder.DropTable(
                name: "TeachersDisciplines");

            migrationBuilder.DropTable(
                name: "TeachingPlanItems");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Disciplines");

            migrationBuilder.DropTable(
                name: "Curriculums");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Careers");

            migrationBuilder.DropTable(
                name: "Faculties");
        }
    }
}
