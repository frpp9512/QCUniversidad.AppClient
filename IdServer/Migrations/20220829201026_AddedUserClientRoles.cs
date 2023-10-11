using Microsoft.EntityFrameworkCore.Migrations;
using System;


namespace IdServer.Migrations;

public partial class AddedUserClientRoles : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "StoredUserClientRoles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Role = table.Column<string>(type: "TEXT", nullable: true),
                StoredClientId = table.Column<Guid>(type: "TEXT", nullable: false),
                StoredUserId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_StoredUserClientRoles", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_StoredUserClientRoles_Clients_StoredClientId",
                    column: x => x.StoredClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_StoredUserClientRoles_StoredUsers_StoredUserId",
                    column: x => x.StoredUserId,
                    principalTable: "StoredUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_StoredUserClientRoles_StoredClientId",
            table: "StoredUserClientRoles",
            column: "StoredClientId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_StoredUserClientRoles_StoredUserId",
            table: "StoredUserClientRoles",
            column: "StoredUserId");
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "StoredUserClientRoles");
}
