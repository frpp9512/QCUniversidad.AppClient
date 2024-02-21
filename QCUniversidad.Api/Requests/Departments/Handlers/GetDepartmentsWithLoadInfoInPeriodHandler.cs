using MediatR;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class GetDepartmentsWithLoadInfoInPeriodHandler : IRequestHandler<GetDepartmentsWithLoadInfoInPeriodRequest, GetDepartmentsWithLoadInfoInPeriodResponse>
{
    public async Task<GetDepartmentsWithLoadInfoInPeriodResponse> Handle(GetDepartmentsWithLoadInfoInPeriodRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
