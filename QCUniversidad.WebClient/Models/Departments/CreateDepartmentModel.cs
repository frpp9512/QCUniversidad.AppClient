using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCUniversidad.WebClient.Models.Faculties;

namespace QCUniversidad.WebClient.Models.Departments
{
    public class CreateDepartmentModel : DepartmentModel
    {
        public IList<FacultyModel>? Faculties { get; set; }
    }
}