using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Models.Teachers;

namespace QCUniversidad.WebClient.Models.LoadDistribution;

public class AddLoadModalModel
{
    public required TeachingPlanItemModel PlanItem { get; set; }
    public required IList<TeacherModel> Teachers { get; set; }
    public double MaxValue { get; set; }
}