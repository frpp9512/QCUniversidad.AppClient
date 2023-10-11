using QCUniversidad.Api.Shared.Dtos.SchoolYear;

namespace QCUniversidad.Api.Shared.Dtos.Period;

public record SimplePeriodDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Starts { get; set; }
    public DateTimeOffset Ends { get; set; }
    public double MonthsCount { get; set; }
    public double TimeFund { get; set; }
    public Guid SchoolYearId { get; set; }
    public required SimpleSchoolYearDto SchoolYear { get; set; }
}