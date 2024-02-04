using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations;

/// <inheritdoc />
public partial class RemovedHaveFinalExamField : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "HaveFinalExam",
            table: "PeriodSubjects");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<bool>(
            name: "HaveFinalExam",
            table: "PeriodSubjects",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }
}
