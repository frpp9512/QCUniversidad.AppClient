using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdServer.Migrations
{
    public partial class AddedPostLogoutRedirectUriToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoredClientPostLogoutRedirectUris",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    StoredClientId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredClientPostLogoutRedirectUris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoredClientPostLogoutRedirectUris_Clients_StoredClientId",
                        column: x => x.StoredClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoredClientPostLogoutRedirectUris_StoredClientId",
                table: "StoredClientPostLogoutRedirectUris",
                column: "StoredClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoredClientPostLogoutRedirectUris");
        }
    }
}
