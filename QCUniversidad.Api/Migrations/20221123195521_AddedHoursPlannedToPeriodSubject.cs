using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QCUniversidad.Api.Migrations;

/// <inheritdoc />
public partial class AddedHoursPlannedToPeriodSubject : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<double>(
            name: "HoursPlanned",
            table: "PeriodSubjects",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "HoursPlanned",
            table: "PeriodSubjects");
    }
}
