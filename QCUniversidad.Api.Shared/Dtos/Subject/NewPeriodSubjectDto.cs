using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Dtos.Period;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Subject;

public record NewPeriodSubjectDto
{
    public Guid PeriodId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid CourseId { get; set; }
    public int MidtermExamsCount { get; set; }
    public bool HaveFinalExam { get; set; }
}