using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.SchoolYears;

namespace QCUniversidad.WebClient.Models.Planning;

public class PlanningIndexModel
{
    public Guid SchoolYearId { get; set; }
    public FacultyModel? Faculty { get; set; }
    public SchoolYearModel? SchoolYear { get; set; }
    public required IList<CareerModel> Careers { get; set; }
    public IList<PeriodModel>? Periods { get; set; }
    public Guid? PeriodSelected { get; set; }
    public Guid? CourseSelected { get; set; }
    public Guid? CareerSelected { get; set; }
    public string Tab { get; set; } = "periodsubjects";
}