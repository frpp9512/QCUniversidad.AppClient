using MediatR;
using QCUniversidad.Api.Requests.Planning.Responses;

namespace QCUniversidad.Api.Requests.Planning.Models;

public class GetPlanningForDepartmentRequest : IRequest<GetPlanningForDepartmentResponse>
{
    public Guid DepartmentId { get; set; }
    public Guid PeriodId { get; set; }
    public bool OnlyLoadItems { get; set; } = false;
    public Guid? CourseId { get; set; }
}
