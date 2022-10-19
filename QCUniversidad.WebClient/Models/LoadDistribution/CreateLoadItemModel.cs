using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.LoadDistribution;
public class CreateLoadItemModel
{
    public Guid TeacherId { get; set; }
    public Guid PlanningItemId { get; set; }
    public double HoursCovered { get; set; }
}
