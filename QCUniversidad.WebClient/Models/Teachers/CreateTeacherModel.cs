using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Disciplines;

namespace QCUniversidad.WebClient.Models.Teachers;

public class CreateTeacherModel : TeacherModel
{
    public IList<DepartmentModel>? DepartmentList { get; set; }
    public new IList<DisciplineModel>? Disciplines { get; set; }
}