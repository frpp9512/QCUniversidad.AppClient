using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations;

/// <inheritdoc />
public partial class TotalHours2 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Subjects",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");

        _ = migrationBuilder.AddColumn<double>(
            name: "TotalHours",
            table: "PeriodSubjects",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "TotalHours",
            table: "PeriodSubjects");

        _ = migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Subjects",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);
    }
}
