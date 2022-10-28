using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Course;

public record NewCourseDto
{
    public Guid SchoolYearId { get; set; }
    public int CareerYear { get; set; }
    public bool LastCourse { get; set; }
    public string Denomination { get; set; }
    public TeachingModality TeachingModality { get; set; }
    public uint Enrolment { get; set; }
    public Guid CareerId { get; set; }
    public Guid CurriculumId { get; set; }
}