using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.SchoolYears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Index;
public class IndexViewModel
{
    public SchoolYearModel SchoolYear { get; set; }
    public DepartmentModel Department { get; set; }
    public FacultyModel Faculty { get; set; }
}
