namespace QCUniversidad.WebClient.Models.LoadDistribution;
public class SetNonTeachingLoadModel
{
    public required string Type { get; set; }
    public required string BaseValue { get; set; }
    public Guid TeacherId { get; set; }
    public Guid PeriodId { get; set; }
}
