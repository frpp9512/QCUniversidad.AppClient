using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations
{
    public partial class AddedSchoolYearEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SchoolYearId",
                table: "Courses",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SchoolYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Current = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolYears", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SchoolYearId",
                table: "Courses",
                column: "SchoolYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_SchoolYears_SchoolYearId",
                table: "Courses",
                column: "SchoolYearId",
                principalTable: "SchoolYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_SchoolYears_SchoolYearId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "SchoolYears");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SchoolYearId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SchoolYearId",
                table: "Courses");
        }
    }
}
