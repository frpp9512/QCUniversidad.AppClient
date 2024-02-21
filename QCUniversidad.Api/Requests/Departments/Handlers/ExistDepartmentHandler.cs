using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class ExistDepartmentHandler(IDepartmentsManager departmentsManager) : IRequestHandler<ExistDepartmentRequest, ExistDepartmentResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;

    public async Task<ExistDepartmentResponse> Handle(ExistDepartmentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _departmentsManager.ExistDepartmentAsync(request.DepartmentId);
            return new()
            {
                DepartmentId = request.DepartmentId,
                Exist = result,
                StatusCode = result ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while determining the existence of department {request.DepartmentId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
