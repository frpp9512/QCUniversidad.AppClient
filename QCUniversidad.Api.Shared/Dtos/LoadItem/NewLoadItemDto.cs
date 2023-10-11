namespace QCUniversidad.Api.Shared.Dtos.LoadItem;

public record NewLoadItemDto
{
    public Guid PlanningItemId { get; set; }
    public Guid TeacherId { get; set; }
    public double HoursCovered { get; set; }
}
