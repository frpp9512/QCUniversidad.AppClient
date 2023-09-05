namespace QCUniversidad.WebClient.Models.LoadDistribution;
public class CreateLoadItemModel
{
    public Guid TeacherId { get; set; }
    public Guid PlanningItemId { get; set; }
    public double HoursCovered { get; set; }
}
