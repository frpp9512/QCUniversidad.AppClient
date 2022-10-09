using QCUniversidad.WebClient.Models.Periods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Planning
{
    public class TeachingPlanModel
    {
        public Guid Id { get; set; }
        public PeriodModel Period { get; set; }
        public Guid PeriodId { get; set; }
        public int ItemsCount { get; set; }
        public int TotalHours { get; set; }
        public IList<TeachingPlanItem> Items { get; set; }
    }
}