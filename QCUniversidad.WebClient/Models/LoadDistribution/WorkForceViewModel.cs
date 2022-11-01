using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.SchoolYears;

namespace QCUniversidad.WebClient.Models.LoadDistribution;

public class WorkForceViewModel
{
    public SchoolYearModel? SchoolYear { get; set; }
    public IList<PeriodModel>? Periods { get; set; }
    public DepartmentModel? Department { get; set; }
}
