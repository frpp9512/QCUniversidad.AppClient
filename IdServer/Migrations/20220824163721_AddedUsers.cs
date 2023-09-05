using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdServer.Migrations
{
    public partial class AddedUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoredUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoredUserClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    ValueType = table.Column<string>(type: "TEXT", nullable: true),
                    Issuer = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StoredUserId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoredUserClaims_StoredUsers_StoredUserId",
                        column: x => x.StoredUserId,
                        principalTable: "StoredUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoredUserSecrets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    StoredUserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredUserSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoredUserSecrets_StoredUsers_StoredUserId",
                        column: x => x.StoredUserId,
                        principalTable: "StoredUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoredUserClaims_StoredUserId",
                table: "StoredUserClaims",
                column: "StoredUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredUserSecrets_StoredUserId",
                table: "StoredUserSecrets",
                column: "StoredUserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoredUserClaims");

            migrationBuilder.DropTable(
                name: "StoredUserSecrets");

            migrationBuilder.DropTable(
                name: "StoredUsers");
        }
    }
}
