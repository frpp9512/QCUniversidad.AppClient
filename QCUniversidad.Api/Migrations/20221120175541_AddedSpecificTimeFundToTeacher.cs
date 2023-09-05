using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations;

/// <inheritdoc />
public partial class AddedSpecificTimeFundToTeacher : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<double>(
            name: "SpecificTimeFund",
            table: "Teachers",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
            name: "SpecificTimeFund",
            table: "Teachers");
}
