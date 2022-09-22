using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations
{
    public partial class RemovedServiceProviderFieldFromTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceProvider",
                table: "Teachers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ServiceProvider",
                table: "Teachers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
