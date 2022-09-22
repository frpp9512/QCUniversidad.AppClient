using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCUniversidad.WebClient.Models.Departments;

namespace QCUniversidad.WebClient.Models.Disciplines
{
    public record CreateDisciplineModel : DisciplineModel
    {
        public IList<DepartmentModel>? Departments { get; set; }
    }
}
