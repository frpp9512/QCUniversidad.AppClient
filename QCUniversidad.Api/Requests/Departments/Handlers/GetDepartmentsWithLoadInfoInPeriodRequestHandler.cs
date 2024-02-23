using MediatR;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class GetDepartmentsWithLoadInfoInPeriodRequestHandler : IRequestHandler<GetDepartmentsWithLoadInfoInPeriodRequest, GetDepartmentsWithLoadInfoInPeriodRequestResponse>
{
    public async Task<GetDepartmentsWithLoadInfoInPeriodRequestResponse> Handle(GetDepartmentsWithLoadInfoInPeriodRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
