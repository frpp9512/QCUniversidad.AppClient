using QCUniversidad.WebClient.Models.Curriculums;

namespace QCUniversidad.WebClient.Models.Course;

public record EditCourseModel : CourseModel
{
    public IList<CurriculumModel>? Curricula { get; set; }
}
