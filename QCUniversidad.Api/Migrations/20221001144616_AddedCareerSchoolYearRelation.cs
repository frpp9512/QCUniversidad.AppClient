using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations
{
    public partial class AddedCareerSchoolYearRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periods_SchoolYears_ShoolYearId",
                table: "Periods");

            migrationBuilder.RenameColumn(
                name: "ShoolYearId",
                table: "Periods",
                newName: "SchoolYearId");

            migrationBuilder.RenameIndex(
                name: "IX_Periods_ShoolYearId",
                table: "Periods",
                newName: "IX_Periods_SchoolYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_SchoolYears_SchoolYearId",
                table: "Periods",
                column: "SchoolYearId",
                principalTable: "SchoolYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periods_SchoolYears_SchoolYearId",
                table: "Periods");

            migrationBuilder.RenameColumn(
                name: "SchoolYearId",
                table: "Periods",
                newName: "ShoolYearId");

            migrationBuilder.RenameIndex(
                name: "IX_Periods_SchoolYearId",
                table: "Periods",
                newName: "IX_Periods_ShoolYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_SchoolYears_ShoolYearId",
                table: "Periods",
                column: "ShoolYearId",
                principalTable: "SchoolYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
