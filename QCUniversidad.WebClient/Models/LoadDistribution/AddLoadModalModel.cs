using QCUniversidad.WebClient.Models.LoadItem;
using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.LoadDistribution;

public class AddLoadModalModel
{
    public TeachingPlanItemModel PlanItem { get; set; }
    public IList<TeacherModel> Teachers { get; set; }
    public double MaxValue { get; set; }
}