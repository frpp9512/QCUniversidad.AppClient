using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Faculties;
using SmartB1t.Security.WebSecurity.Local.Models;

namespace QCUniversidad.WebClient.Models.Accounts;

public class UserViewModel : User
{
    public required DepartmentModel DepartmentModel { get; set; }
    public required FacultyModel FacultyModel { get; set; }
}
