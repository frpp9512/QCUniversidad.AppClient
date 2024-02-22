namespace QCUniversidad.Api.Shared.Dtos.LoadItem;

public record SetNonTeachingLoadDto
{
    public string? Type { get; set; }
    public string? BaseValue { get; set; }
    public Guid TeacherId { get; set; }
    public Guid PeriodId { get; set; }
}
