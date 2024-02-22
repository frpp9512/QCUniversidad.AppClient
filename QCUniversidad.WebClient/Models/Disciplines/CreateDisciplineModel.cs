using QCUniversidad.WebClient.Models.Departments;

namespace QCUniversidad.WebClient.Models.Disciplines;

public record CreateDisciplineModel : DisciplineModel
{
    public IList<DepartmentModel>? Departments { get; set; }
}
