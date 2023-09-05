using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdServer.Migrations
{
    public partial class AddedResourceStoreWithApiAndIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityResources",
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
                    table.PrimaryKey("PK_IdentityResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityResourceUserClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserClaim = table.Column<string>(type: "TEXT", nullable: true),
                    StoredIdentityResourceId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityResourceUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityResourceUserClaims_IdentityResources_StoredIdentityResourceId",
                        column: x => x.StoredIdentityResourceId,
                        principalTable: "IdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityResourceUserClaims_StoredIdentityResourceId",
                table: "IdentityResourceUserClaims",
                column: "StoredIdentityResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityResourceUserClaims");

            migrationBuilder.DropTable(
                name: "IdentityResources");
        }
    }
}
