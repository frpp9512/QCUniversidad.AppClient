using QCUniversidad.WebClient.Models.Faculties;

namespace QCUniversidad.WebClient.Models.Careers;

public class CreateCareerModel : CareerModel
{
    public IList<FacultyModel>? Faculties { get; set; }
}
