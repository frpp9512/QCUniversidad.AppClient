using Microsoft.EntityFrameworkCore.Migrations;
using System;


namespace IdServer.Migrations;

public partial class CreatingDatabase : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "Clients",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                ClientId = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_Clients", x => x.Id));

        _ = migrationBuilder.CreateTable(
            name: "ClientSecrets",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Description = table.Column<string>(type: "TEXT", nullable: true),
                Value = table.Column<string>(type: "TEXT", nullable: true),
                Expiration = table.Column<DateTime>(type: "TEXT", nullable: true),
                ClientId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ClientSecrets", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ClientSecrets_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "StoredClientAllowedScope",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                AllowedScope = table.Column<string>(type: "TEXT", nullable: true),
                ClientId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_StoredClientAllowedScope", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_StoredClientAllowedScope_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id");
            });

        _ = migrationBuilder.CreateTable(
            name: "StoredClientGrantType",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                GrantType = table.Column<string>(type: "TEXT", nullable: true),
                ClientId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_StoredClientGrantType", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_StoredClientGrantType_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id");
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_ClientSecrets_ClientId",
            table: "ClientSecrets",
            column: "ClientId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_StoredClientAllowedScope_ClientId",
            table: "StoredClientAllowedScope",
            column: "ClientId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_StoredClientGrantType_ClientId",
            table: "StoredClientGrantType",
            column: "ClientId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "ClientSecrets");

        _ = migrationBuilder.DropTable(
            name: "StoredClientAllowedScope");

        _ = migrationBuilder.DropTable(
            name: "StoredClientGrantType");

        _ = migrationBuilder.DropTable(
            name: "Clients");
    }
}
