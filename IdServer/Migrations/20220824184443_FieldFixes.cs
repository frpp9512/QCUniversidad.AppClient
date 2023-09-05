using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdServer.Migrations
{
    public partial class FieldFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "IdentityResources",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowInDiscoveryDocument",
                table: "IdentityResources",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "IdentityResources");

            migrationBuilder.DropColumn(
                name: "ShowInDiscoveryDocument",
                table: "IdentityResources");
        }
    }
}
