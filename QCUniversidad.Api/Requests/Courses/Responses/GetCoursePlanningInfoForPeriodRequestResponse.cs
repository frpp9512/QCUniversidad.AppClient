using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record GetCoursePlanningInfoForPeriodRequestResponse : RequestResponseBase
{
    public CoursePeriodPlanningInfoDto? PlanningInfo { get; set; }
    public override object? GetPayload() => PlanningInfo;
}
