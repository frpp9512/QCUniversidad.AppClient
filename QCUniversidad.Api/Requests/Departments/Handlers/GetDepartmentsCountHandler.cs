using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class GetDepartmentsCountHandler(IDepartmentsManager departmentsManager) : IRequestHandler<GetDepartmentsCountRequest, GetDepartmentsCountResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;

    public async Task<GetDepartmentsCountResponse> Handle(GetDepartmentsCountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            int count = await _departmentsManager.GetDepartmentsCountAsync();
            return new()
            {
                Count = count
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while fetching the count of departments. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
