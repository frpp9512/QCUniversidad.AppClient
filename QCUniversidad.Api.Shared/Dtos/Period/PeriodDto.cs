using QCUniversidad.Api.Shared.Dtos.Career;
using QCUniversidad.Api.Shared.Dtos.Curriculum;
using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Period;

public record PeriodDto
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Starts { get; set; }
    public DateTimeOffset Ends { get; set; }
    public uint Enrolment { get; set; }
    public double TimeFund { get; set; }
    public Guid CourseId { get; set; }
    public CourseDto Course { get; set; }
}