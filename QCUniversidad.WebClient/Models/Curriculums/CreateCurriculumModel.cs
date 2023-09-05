using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Disciplines;

namespace QCUniversidad.WebClient.Models.Curriculums;

public class CreateCurriculumModel : CurriculumModel
{
    public IList<CareerModel>? Careers { get; set; }
    public IList<DisciplineModel>? Disciplines { get; set; }
}