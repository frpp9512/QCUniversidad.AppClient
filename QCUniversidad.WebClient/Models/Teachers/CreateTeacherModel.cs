using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Disciplines;

namespace QCUniversidad.WebClient.Models.Teachers
{
    public class CreateTeacherModel : TeacherModel
    {
        public IList<DepartmentModel>? DepartmentList { get; set; }
        public IList<DisciplineModel>? Disciplines { get; set; }
    }
}