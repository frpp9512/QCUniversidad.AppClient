using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdServer.Migrations
{
    public partial class AddedApiScopesToResourceStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    Required = table.Column<bool>(type: "INTEGER", nullable: false),
                    Emphasize = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "storedApiScopeUserClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserClaim = table.Column<string>(type: "TEXT", nullable: true),
                    StoredApiScopeId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_storedApiScopeUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_storedApiScopeUserClaims_ApiScopes_StoredApiScopeId",
                        column: x => x.StoredApiScopeId,
                        principalTable: "ApiScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_storedApiScopeUserClaims_StoredApiScopeId",
                table: "storedApiScopeUserClaims",
                column: "StoredApiScopeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "storedApiScopeUserClaims");

            migrationBuilder.DropTable(
                name: "ApiScopes");
        }
    }
}
