using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class GetCoursePlanningInfoForPeriodRequest : RequestBase<GetCoursePlanningInfoForPeriodRequestResponse>
{
    public Guid CourseId { get; set; }
    public Guid PeriodId { get; set; }
}
