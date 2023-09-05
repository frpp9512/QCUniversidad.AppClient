using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Curriculums;
using QCUniversidad.WebClient.Models.SchoolYears;

namespace QCUniversidad.WebClient.Models.Course;

public record CreateCourseModel : CourseModel
{
    public IList<SchoolYearModel>? SchoolYears { get; set; }
    public IList<CareerModel>? Careers { get; set; }
    public IList<CurriculumModel>? Curricula { get; set; }
}