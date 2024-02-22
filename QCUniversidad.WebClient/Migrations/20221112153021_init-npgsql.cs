using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.WebClient.Migrations;

public partial class initnpgsql : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "text", nullable: true),
                Description = table.Column<string>(type: "text", nullable: true),
                Active = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Roles", x => x.Id));

        _ = migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ProfilePicture = table.Column<byte[]>(type: "bytea", nullable: true),
                Fullname = table.Column<string>(type: "text", nullable: true),
                Position = table.Column<string>(type: "text", nullable: true),
                Department = table.Column<string>(type: "text", nullable: true),
                Email = table.Column<string>(type: "text", nullable: true),
                Active = table.Column<bool>(type: "boolean", nullable: false),
                PermanentDeactivation = table.Column<bool>(type: "boolean", nullable: false),
                UserSecretsId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Users", x => x.Id));

        _ = migrationBuilder.CreateTable(
            name: "ExtraClaims",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Type = table.Column<string>(type: "text", nullable: true),
                Value = table.Column<string>(type: "text", nullable: true),
                UserId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ExtraClaims", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ExtraClaims_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "Secrets",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Password = table.Column<string>(type: "text", nullable: true),
                UserId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Secrets", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_Secrets_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "UserRoles",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                RoleId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                _ = table.ForeignKey(
                    name: "FK_UserRoles_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                    name: "FK_UserRoles_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_ExtraClaims_UserId",
            table: "ExtraClaims",
            column: "UserId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Secrets_UserId",
            table: "Secrets",
            column: "UserId",
            unique: true);

        _ = migrationBuilder.CreateIndex(
            name: "IX_UserRoles_RoleId",
            table: "UserRoles",
            column: "RoleId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "ExtraClaims");

        _ = migrationBuilder.DropTable(
            name: "Secrets");

        _ = migrationBuilder.DropTable(
            name: "UserRoles");

        _ = migrationBuilder.DropTable(
            name: "Roles");

        _ = migrationBuilder.DropTable(
            name: "Users");
    }
}
