using Microsoft.EntityFrameworkCore.Migrations;


namespace IdServer.Migrations;

public partial class AddedUserClaimsInTokenAndConsentAsk : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<bool>(
            name: "AlwaysIncludeUserClaimsInIdToken",
            table: "Clients",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);

        _ = migrationBuilder.AddColumn<bool>(
            name: "RequireConsent",
            table: "Clients",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "AlwaysIncludeUserClaimsInIdToken",
            table: "Clients");

        _ = migrationBuilder.DropColumn(
            name: "RequireConsent",
            table: "Clients");
    }
}
