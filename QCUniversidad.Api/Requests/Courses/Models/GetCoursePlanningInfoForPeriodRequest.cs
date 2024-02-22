using MediatR;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class GetCoursePlanningInfoForPeriodRequest : IRequest<GetCoursePlanningInfoForPeriodResponse>
{
    public Guid CourseId { get; set; }
    public Guid PeriodId { get; set; }
}
