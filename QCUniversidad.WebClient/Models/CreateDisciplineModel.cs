using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models
{
    public record CreateDisciplineModel : DisciplineModel
    {
        public IList<DepartmentModel>? Departments { get; set; }
    }
}
