using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models
{
    public class CreateDepartmentModel : DepartmentModel
    {
        public IList<FacultyModel>? Faculties { get; set; }
    }
}
