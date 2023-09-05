using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdServer.Migrations
{
    public partial class AddedApiResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoredClientAllowedScope_Clients_ClientId",
                table: "StoredClientAllowedScope");

            migrationBuilder.DropForeignKey(
                name: "FK_StoredClientGrantType_Clients_ClientId",
                table: "StoredClientGrantType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoredClientGrantType",
                table: "StoredClientGrantType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoredClientAllowedScope",
                table: "StoredClientAllowedScope");

            migrationBuilder.RenameTable(
                name: "StoredClientGrantType",
                newName: "ClientGrantTypes");

            migrationBuilder.RenameTable(
                name: "StoredClientAllowedScope",
                newName: "ClientAllowedScopes");

            migrationBuilder.RenameIndex(
                name: "IX_StoredClientGrantType_ClientId",
                table: "ClientGrantTypes",
                newName: "IX_ClientGrantTypes_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_StoredClientAllowedScope_ClientId",
                table: "ClientAllowedScopes",
                newName: "IX_ClientAllowedScopes_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientGrantTypes",
                table: "ClientGrantTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientAllowedScopes",
                table: "ClientAllowedScopes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ApiResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiResourceScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Scope = table.Column<string>(type: "TEXT", nullable: true),
                    StoredApiResourceId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResourceScopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiResourceScopes_ApiResources_StoredApiResourceId",
                        column: x => x.StoredApiResourceId,
                        principalTable: "ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "apiResourceUserClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserClaim = table.Column<string>(type: "TEXT", nullable: true),
                    ApiResourceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StoredApiResourceId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apiResourceUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_apiResourceUserClaims_ApiResources_StoredApiResourceId",
                        column: x => x.StoredApiResourceId,
                        principalTable: "ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiResourceScopes_StoredApiResourceId",
                table: "ApiResourceScopes",
                column: "StoredApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_apiResourceUserClaims_StoredApiResourceId",
                table: "apiResourceUserClaims",
                column: "StoredApiResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAllowedScopes_Clients_ClientId",
                table: "ClientAllowedScopes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientGrantTypes_Clients_ClientId",
                table: "ClientGrantTypes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAllowedScopes_Clients_ClientId",
                table: "ClientAllowedScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientGrantTypes_Clients_ClientId",
                table: "ClientGrantTypes");

            migrationBuilder.DropTable(
                name: "ApiResourceScopes");

            migrationBuilder.DropTable(
                name: "apiResourceUserClaims");

            migrationBuilder.DropTable(
                name: "ApiResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientGrantTypes",
                table: "ClientGrantTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientAllowedScopes",
                table: "ClientAllowedScopes");

            migrationBuilder.RenameTable(
                name: "ClientGrantTypes",
                newName: "StoredClientGrantType");

            migrationBuilder.RenameTable(
                name: "ClientAllowedScopes",
                newName: "StoredClientAllowedScope");

            migrationBuilder.RenameIndex(
                name: "IX_ClientGrantTypes_ClientId",
                table: "StoredClientGrantType",
                newName: "IX_StoredClientGrantType_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientAllowedScopes_ClientId",
                table: "StoredClientAllowedScope",
                newName: "IX_StoredClientAllowedScope_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoredClientGrantType",
                table: "StoredClientGrantType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoredClientAllowedScope",
                table: "StoredClientAllowedScope",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoredClientAllowedScope_Clients_ClientId",
                table: "StoredClientAllowedScope",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoredClientGrantType_Clients_ClientId",
                table: "StoredClientGrantType",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
