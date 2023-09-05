using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Models.Teachers;

namespace QCUniversidad.WebClient.Models.LoadItem;

public class LoadItemModel
{
    public Guid Id { get; set; }
    public Guid PlanningItemId { get; set; }
    public required TeachingPlanItemModel PlanningItem { get; set; }
    public Guid TeacherId { get; set; }
    public required TeacherModel Teacher { get; set; }
    public double HoursCovered { get; set; }
}
