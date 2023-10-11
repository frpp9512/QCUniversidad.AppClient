using QCUniversidad.Api.Shared.Dtos.Period;

namespace QCUniversidad.Api.Shared.Dtos.Course;

public record CoursePeriodPlanningInfoDto
{
    public required Guid PeriodId { get; set; }
    public required SimplePeriodDto Period { get; set; }
    public required Guid CourseId { get; set; }
    public SimpleCourseDto? Course { get; set; }
    public double TotalHoursPlanned { get; set; }
    public double TotalHoursPlannedPercent => Math.Round(TotalHoursPlanned / Period.TimeFund, 2);

    public double TotalHoursPlannedCourseByMeetingPercent => Math.Round(TotalHoursPlanned / CourseByMeetingTimeFund, 2);

    public double CourseByMeetingTimeFund => CourseByMeetingsCount * 2 * 8;

    public double CourseByMeetingsCount
    {
        get
        {
            var dateDiff = Period.Ends - Period.Starts;
            var weekends = Math.Floor(Math.Ceiling((double)(dateDiff.TotalDays / 7)) / 2);
            return weekends;
        }
    }

    public double RealHoursPlanned { get; set; }
    public double RealHoursPlannedPercent => TotalHoursPlanned > 0 ? Math.Round(RealHoursPlanned / TotalHoursPlanned, 2) : 0;
}