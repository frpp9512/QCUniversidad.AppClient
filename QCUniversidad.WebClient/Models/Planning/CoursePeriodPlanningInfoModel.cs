using QCUniversidad.WebClient.Models.Course;
using QCUniversidad.WebClient.Models.Periods;

namespace QCUniversidad.WebClient.Models.Planning;

public class CoursePeriodPlanningInfoModel
{
    public required Guid PeriodId { get; set; }
    public required PeriodModel Period { get; set; }
    public required Guid CourseId { get; set; }
    public CourseModel? Course { get; set; }
    public double TotalHoursPlanned { get; set; }
    public double TotalHoursPlannedPercent { get; set; }
    public double TotalHoursPlannedCourseByMeetingPercent { get; set; }
    public double CourseByMeetingTimeFund { get; set; }
    public double CourseByMeetingsCount { get; set; }
    public bool IsNotLoadGenerator { get; set; }
    public double RealHoursPlanned { get; set; }
    public double RealHoursPlannedPercent { get; set; }
}