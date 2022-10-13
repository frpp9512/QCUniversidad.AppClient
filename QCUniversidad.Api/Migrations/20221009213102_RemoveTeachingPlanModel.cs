using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations
{
    public partial class RemoveTeachingPlanModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeachingPlanItems_TeachingPlans_TeachingPlanId",
                table: "TeachingPlanItems");

            migrationBuilder.DropTable(
                name: "TeachingPlans");

            migrationBuilder.RenameColumn(
                name: "TeachingPlanId",
                table: "TeachingPlanItems",
                newName: "PeriodId");

            migrationBuilder.RenameIndex(
                name: "IX_TeachingPlanItems_TeachingPlanId",
                table: "TeachingPlanItems",
                newName: "IX_TeachingPlanItems_PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingPlanItems_Periods_PeriodId",
                table: "TeachingPlanItems",
                column: "PeriodId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeachingPlanItems_Periods_PeriodId",
                table: "TeachingPlanItems");

            migrationBuilder.RenameColumn(
                name: "PeriodId",
                table: "TeachingPlanItems",
                newName: "TeachingPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_TeachingPlanItems_PeriodId",
                table: "TeachingPlanItems",
                newName: "IX_TeachingPlanItems_TeachingPlanId");

            migrationBuilder.CreateTable(
                name: "TeachingPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PeriodId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachingPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeachingPlans_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeachingPlans_PeriodId",
                table: "TeachingPlans",
                column: "PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingPlanItems_TeachingPlans_TeachingPlanId",
                table: "TeachingPlanItems",
                column: "TeachingPlanId",
                principalTable: "TeachingPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
