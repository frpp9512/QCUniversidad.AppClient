using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations
{
    public partial class AddedRelationshipCareerCurriculum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CareerId",
                table: "Curriculums",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Curriculums_CareerId",
                table: "Curriculums",
                column: "CareerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Curriculums_Careers_CareerId",
                table: "Curriculums",
                column: "CareerId",
                principalTable: "Careers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Curriculums_Careers_CareerId",
                table: "Curriculums");

            migrationBuilder.DropIndex(
                name: "IX_Curriculums_CareerId",
                table: "Curriculums");

            migrationBuilder.DropColumn(
                name: "CareerId",
                table: "Curriculums");
        }
    }
}
