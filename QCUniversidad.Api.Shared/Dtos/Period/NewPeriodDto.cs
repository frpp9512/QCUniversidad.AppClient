namespace QCUniversidad.Api.Shared.Dtos.Period;

public record NewPeriodDto
{
    public string? Description { get; set; }
    public DateTimeOffset Starts { get; set; }
    public DateTimeOffset Ends { get; set; }
    public double MonthsCount { get; set; }
    public Guid SchoolYearId { get; set; }
}