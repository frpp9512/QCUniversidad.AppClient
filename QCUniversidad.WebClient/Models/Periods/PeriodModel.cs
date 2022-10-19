using QCUniversidad.WebClient.Models.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Periods;

/// <summary>
/// Represents a period inside the school year, when the students will recieve the subjects.
/// </summary>
public record PeriodModel
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Starts { get; set; }
    public DateTimeOffset Ends { get; set; }
    public uint Enrolment { get; set; }
    public double TimeFund { get; set; }
    public Guid CourseId { get; set; }
    public CourseModel? Course { get; set; }
}