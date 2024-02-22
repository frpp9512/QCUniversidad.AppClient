using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations;

/// <inheritdoc />
public partial class AddedIsStudyCenterToDepartment : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Departments",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");

        _ = migrationBuilder.AlterColumn<string>(
            name: "InternalId",
            table: "Departments",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");

        _ = migrationBuilder.AddColumn<bool>(
            name: "IsStudyCenter",
            table: "Departments",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "IsStudyCenter",
            table: "Departments");

        _ = migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Departments",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);

        _ = migrationBuilder.AlterColumn<string>(
            name: "InternalId",
            table: "Departments",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);
    }
}
