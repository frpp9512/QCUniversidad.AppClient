using QCUniversidad.WebClient.Models.Courses;
using QCUniversidad.WebClient.Models.Periods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Subjects;

public class CreatePeriodSubjectModel
{
    public Guid PeriodId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid CourseId { get; set; }
    public int MidtermExamsCount { get; set; }
    public bool HaveFinalExam { get; set; }
}