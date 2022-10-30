using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.LoadDistribution;
public class SetNonTeachingLoadModel
{
    public string Type { get; set; }
    public string BaseValue { get; set; }
    public Guid TeacherId { get; set; }
    public Guid PeriodId { get; set; }
}
