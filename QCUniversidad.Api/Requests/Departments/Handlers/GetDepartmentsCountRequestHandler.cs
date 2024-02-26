using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class GetDepartmentsCountRequestHandler(IDepartmentsManager departmentsManager) : IRequestHandler<GetDepartmentsCountRequest, GetDepartmentsCountRequestResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;

    public async Task<GetDepartmentsCountRequestResponse> Handle(GetDepartmentsCountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            int count = await _departmentsManager.GetDepartmentsCountAsync();
            return new()
            {
                RequestId = request.RequestId,
                Count = count
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while fetching the count of departments. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
