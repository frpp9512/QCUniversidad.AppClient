using Microsoft.EntityFrameworkCore.Migrations;
using System;


namespace IdServer.Migrations;

public partial class AddedApiScopesToResourceStore : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "ApiScopes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: true),
                DisplayName = table.Column<string>(type: "TEXT", nullable: true),
                Required = table.Column<bool>(type: "INTEGER", nullable: false),
                Emphasize = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_ApiScopes", x => x.Id));

        _ = migrationBuilder.CreateTable(
            name: "storedApiScopeUserClaims",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                UserClaim = table.Column<string>(type: "TEXT", nullable: true),
                StoredApiScopeId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_storedApiScopeUserClaims", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_storedApiScopeUserClaims_ApiScopes_StoredApiScopeId",
                    column: x => x.StoredApiScopeId,
                    principalTable: "ApiScopes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_storedApiScopeUserClaims_StoredApiScopeId",
            table: "storedApiScopeUserClaims",
            column: "StoredApiScopeId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "storedApiScopeUserClaims");

        _ = migrationBuilder.DropTable(
            name: "ApiScopes");
    }
}
