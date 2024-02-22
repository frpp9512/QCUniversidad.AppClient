using QCUniversidad.WebClient.Models.Teachers;

namespace QCUniversidad.WebClient.Models.LoadDistribution;

public class TeachersViewModel
{
    public Guid PeriodId { get; set; }
    public required IList<TeacherModel> DepartmentsTeacher { get; set; }
    public IList<TeacherModel>? SupportTeachers { get; set; }
}