using QCUniversidad.WebClient.Models.Courses;
using QCUniversidad.WebClient.Models.Periods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Subjects;

public class PeriodSubjectModel
{
    public Guid? Id { get; set; }
    public Guid PeriodId { get; set; }
    public PeriodModel? Period { get; set; }
    public Guid SubjectId { get; set; }
    public SubjectModel? Subject { get; set; }
    public Guid CourseId { get; set; }
    public CourseModel? Course { get; set; }
    public int MidtermExamsCount { get; set; }
    public bool HaveFinalExam { get; set; }
}
