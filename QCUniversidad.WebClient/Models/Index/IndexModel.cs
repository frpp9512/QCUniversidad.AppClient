using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.SchoolYears;

namespace QCUniversidad.WebClient.Models.Index;
public class IndexViewModel
{
    public required SchoolYearModel SchoolYear { get; set; }
    public required DepartmentModel Department { get; set; }
    public required FacultyModel Faculty { get; set; }
}
