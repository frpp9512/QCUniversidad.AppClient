using Microsoft.EntityFrameworkCore.Migrations;
using System;


namespace IdServer.Migrations;

public partial class AddedApiResources : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropForeignKey(
            name: "FK_StoredClientAllowedScope_Clients_ClientId",
            table: "StoredClientAllowedScope");

        _ = migrationBuilder.DropForeignKey(
            name: "FK_StoredClientGrantType_Clients_ClientId",
            table: "StoredClientGrantType");

        _ = migrationBuilder.DropPrimaryKey(
            name: "PK_StoredClientGrantType",
            table: "StoredClientGrantType");

        _ = migrationBuilder.DropPrimaryKey(
            name: "PK_StoredClientAllowedScope",
            table: "StoredClientAllowedScope");

        _ = migrationBuilder.RenameTable(
            name: "StoredClientGrantType",
            newName: "ClientGrantTypes");

        _ = migrationBuilder.RenameTable(
            name: "StoredClientAllowedScope",
            newName: "ClientAllowedScopes");

        _ = migrationBuilder.RenameIndex(
            name: "IX_StoredClientGrantType_ClientId",
            table: "ClientGrantTypes",
            newName: "IX_ClientGrantTypes_ClientId");

        _ = migrationBuilder.RenameIndex(
            name: "IX_StoredClientAllowedScope_ClientId",
            table: "ClientAllowedScopes",
            newName: "IX_ClientAllowedScopes_ClientId");

        _ = migrationBuilder.AddPrimaryKey(
            name: "PK_ClientGrantTypes",
            table: "ClientGrantTypes",
            column: "Id");

        _ = migrationBuilder.AddPrimaryKey(
            name: "PK_ClientAllowedScopes",
            table: "ClientAllowedScopes",
            column: "Id");

        _ = migrationBuilder.CreateTable(
            name: "ApiResources",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: true),
                DisplayName = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_ApiResources", x => x.Id));

        _ = migrationBuilder.CreateTable(
            name: "ApiResourceScopes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Scope = table.Column<string>(type: "TEXT", nullable: true),
                StoredApiResourceId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ApiResourceScopes", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ApiResourceScopes_ApiResources_StoredApiResourceId",
                    column: x => x.StoredApiResourceId,
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
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
                _ = table.PrimaryKey("PK_apiResourceUserClaims", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_apiResourceUserClaims_ApiResources_StoredApiResourceId",
                    column: x => x.StoredApiResourceId,
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_ApiResourceScopes_StoredApiResourceId",
            table: "ApiResourceScopes",
            column: "StoredApiResourceId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_apiResourceUserClaims_StoredApiResourceId",
            table: "apiResourceUserClaims",
            column: "StoredApiResourceId");

        _ = migrationBuilder.AddForeignKey(
            name: "FK_ClientAllowedScopes_Clients_ClientId",
            table: "ClientAllowedScopes",
            column: "ClientId",
            principalTable: "Clients",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        _ = migrationBuilder.AddForeignKey(
            name: "FK_ClientGrantTypes_Clients_ClientId",
            table: "ClientGrantTypes",
            column: "ClientId",
            principalTable: "Clients",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropForeignKey(
            name: "FK_ClientAllowedScopes_Clients_ClientId",
            table: "ClientAllowedScopes");

        _ = migrationBuilder.DropForeignKey(
            name: "FK_ClientGrantTypes_Clients_ClientId",
            table: "ClientGrantTypes");

        _ = migrationBuilder.DropTable(
            name: "ApiResourceScopes");

        _ = migrationBuilder.DropTable(
            name: "apiResourceUserClaims");

        _ = migrationBuilder.DropTable(
            name: "ApiResources");

        _ = migrationBuilder.DropPrimaryKey(
            name: "PK_ClientGrantTypes",
            table: "ClientGrantTypes");

        _ = migrationBuilder.DropPrimaryKey(
            name: "PK_ClientAllowedScopes",
            table: "ClientAllowedScopes");

        _ = migrationBuilder.RenameTable(
            name: "ClientGrantTypes",
            newName: "StoredClientGrantType");

        _ = migrationBuilder.RenameTable(
            name: "ClientAllowedScopes",
            newName: "StoredClientAllowedScope");

        _ = migrationBuilder.RenameIndex(
            name: "IX_ClientGrantTypes_ClientId",
            table: "StoredClientGrantType",
            newName: "IX_StoredClientGrantType_ClientId");

        _ = migrationBuilder.RenameIndex(
            name: "IX_ClientAllowedScopes_ClientId",
            table: "StoredClientAllowedScope",
            newName: "IX_StoredClientAllowedScope_ClientId");

        _ = migrationBuilder.AddPrimaryKey(
            name: "PK_StoredClientGrantType",
            table: "StoredClientGrantType",
            column: "Id");

        _ = migrationBuilder.AddPrimaryKey(
            name: "PK_StoredClientAllowedScope",
            table: "StoredClientAllowedScope",
            column: "Id");

        _ = migrationBuilder.AddForeignKey(
            name: "FK_StoredClientAllowedScope_Clients_ClientId",
            table: "StoredClientAllowedScope",
            column: "ClientId",
            principalTable: "Clients",
            principalColumn: "Id");

        _ = migrationBuilder.AddForeignKey(
            name: "FK_StoredClientGrantType_Clients_ClientId",
            table: "StoredClientGrantType",
            column: "ClientId",
            principalTable: "Clients",
            principalColumn: "Id");
    }
}
