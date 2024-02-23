using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentsWithLoadInfoInPeriodRequest : IRequest<GetDepartmentsWithLoadInfoInPeriodRequestResponse>
{
    public Guid PeriodId { get; set; }
}
