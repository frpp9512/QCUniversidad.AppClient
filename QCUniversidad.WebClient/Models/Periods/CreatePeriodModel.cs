using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Periods
{
    public record CreatePeriodModel
    {
        public int OrderNumber { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset Starts { get; set; }
        public DateTimeOffset Ends { get; set; }
        public uint Enrolment { get; set; }
        public Guid SchoolYearId { get; set; }
    }
}
