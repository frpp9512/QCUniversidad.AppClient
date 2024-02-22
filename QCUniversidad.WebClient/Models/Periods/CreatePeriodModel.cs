namespace QCUniversidad.WebClient.Models.Periods;

public record CreatePeriodModel
{
    public string? Description { get; set; }
    public DateTimeOffset Starts { get; set; }
    public DateTimeOffset Ends { get; set; }
    public double MonthsCount { get; set; }
    public Guid SchoolYearId { get; set; }
}
