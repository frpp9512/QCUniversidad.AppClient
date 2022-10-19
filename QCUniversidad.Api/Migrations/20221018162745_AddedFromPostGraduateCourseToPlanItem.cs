using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations
{
    public partial class AddedFromPostGraduateCourseToPlanItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FromPostgraduateCourse",
                table: "TeachingPlanItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromPostgraduateCourse",
                table: "TeachingPlanItems");
        }
    }
}
