using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentsWithLoadInfoInPeriodRequest : IRequest<GetDepartmentsWithLoadInfoInPeriodResponse>
{
    public Guid PeriodId { get; set; }
}
