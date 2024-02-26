using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentsWithLoadInfoInPeriodRequest : RequestBase<GetDepartmentsWithLoadInfoInPeriodRequestResponse>
{
    public Guid PeriodId { get; set; }
}
