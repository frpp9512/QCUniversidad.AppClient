using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations
{
    public partial class MovedPeriodToSchoolYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periods_Courses_CourseId",
                table: "Periods");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Periods",
                newName: "SchoolYearId");

            migrationBuilder.RenameIndex(
                name: "IX_Periods_CourseId",
                table: "Periods",
                newName: "IX_Periods_SchoolYearId");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "TeachingPlanItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TeachingPlanItems_CourseId",
                table: "TeachingPlanItems",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_SchoolYears_SchoolYearId",
                table: "Periods",
                column: "SchoolYearId",
                principalTable: "SchoolYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingPlanItems_Courses_CourseId",
                table: "TeachingPlanItems",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periods_SchoolYears_SchoolYearId",
                table: "Periods");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachingPlanItems_Courses_CourseId",
                table: "TeachingPlanItems");

            migrationBuilder.DropIndex(
                name: "IX_TeachingPlanItems_CourseId",
                table: "TeachingPlanItems");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "TeachingPlanItems");

            migrationBuilder.RenameColumn(
                name: "SchoolYearId",
                table: "Periods",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Periods_SchoolYearId",
                table: "Periods",
                newName: "IX_Periods_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_Courses_CourseId",
                table: "Periods",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
