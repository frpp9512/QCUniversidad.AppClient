using QCUniversidad.WebClient.Models.Faculties;

namespace QCUniversidad.WebClient.Models.Departments;

public class CreateDepartmentModel : DepartmentModel
{
    public IList<FacultyModel>? Faculties { get; set; }
}