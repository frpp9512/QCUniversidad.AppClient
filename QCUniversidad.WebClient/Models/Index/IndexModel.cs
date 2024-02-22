using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.SchoolYears;

namespace QCUniversidad.WebClient.Models.Index;
public class IndexViewModel
{
    public SchoolYearModel? SchoolYear { get; set; }
    public DepartmentModel? Department { get; set; }
    public FacultyModel? Faculty { get; set; }
}
