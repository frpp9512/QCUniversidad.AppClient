using Microsoft.EntityFrameworkCore.Migrations;


namespace IdServer.Migrations;

public partial class FieldFixes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<string>(
            name: "Description",
            table: "IdentityResources",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<bool>(
            name: "ShowInDiscoveryDocument",
            table: "IdentityResources",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "Description",
            table: "IdentityResources");

        _ = migrationBuilder.DropColumn(
            name: "ShowInDiscoveryDocument",
            table: "IdentityResources");
    }
}
