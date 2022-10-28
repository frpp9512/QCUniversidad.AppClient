using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.ConfigurationModels
{
    public class CalculationOptions
    {
        public double PregraduateTotalHoursCoefficient { get; set; }
        public double PostgraduateTotalHoursCoefficient { get; set; }
        public double RAPReference { get; set; }
        public double MonthTimeFund { get; set; }
        public double ClassPreparationPrimaryCoefficient { get; set; } = 2;
        public double ClassPreparationSecondaryCoefficient { get; set; } = 1;
        public double ConsultationCoefficient { get; set; } = 8;
        public double MeetingsCoefficient { get; set; } = 1;
        public double MethodologicalActionsCoefficient { get; set; } = 16;
        public double EventsAndPublicationsCoefficient { get; set; } = 15;
        public double OtherActivitiesCoefficient { get; set; } = 3;
    }
}
