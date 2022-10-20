using QCUniversidad.WebClient.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.LoadDistribution;

public class TeachersViewModel
{
    public IList<TeacherModel> DepartmentsTeacher { get; set; }
    public IList<TeacherModel> SupportTeachers { get; set; }
}
