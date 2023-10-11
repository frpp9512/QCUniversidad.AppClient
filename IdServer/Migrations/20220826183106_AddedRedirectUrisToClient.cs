using Microsoft.EntityFrameworkCore.Migrations;
using System;


namespace IdServer.Migrations;

public partial class AddedRedirectUrisToClient : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "StoredClientRedirectUris",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Url = table.Column<string>(type: "TEXT", nullable: true),
                StoredClientId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_StoredClientRedirectUris", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_StoredClientRedirectUris_Clients_StoredClientId",
                    column: x => x.StoredClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_StoredClientRedirectUris_StoredClientId",
            table: "StoredClientRedirectUris",
            column: "StoredClientId");
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "StoredClientRedirectUris");
}
