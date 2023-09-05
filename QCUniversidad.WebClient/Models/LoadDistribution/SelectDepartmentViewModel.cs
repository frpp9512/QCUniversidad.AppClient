using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.SchoolYears;

namespace QCUniversidad.WebClient.Models.LoadDistribution;

public class SelectDepartmentViewModel
{
    public required IList<SchoolYearModel> SchoolYears { get; set; }
    public required IList<DepartmentModel> Departments { get; set; }
    public required string RedirectTo { get; set; }
}
