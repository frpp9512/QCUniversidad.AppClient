using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.LoadItem
{
    public class LoadItemModel
    {
        public Guid Id { get; set; }
        public Guid PlanningItemId { get; set; }
        public TeachingPlanItemModel PlanningItem { get; set; }
        public Guid TeacherId { get; set; }
        public TeacherModel Teacher { get; set; }
        public double HoursCovered { get; set; }
    }
}
