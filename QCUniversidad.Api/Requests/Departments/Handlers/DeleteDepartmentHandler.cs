using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class DeleteDepartmentHandler(IDepartmentsManager departmentsManager) : IRequestHandler<DeleteDepartmentRequest, DeleteDepartmentResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;

    public async Task<DeleteDepartmentResponse> Handle(DeleteDepartmentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _departmentsManager.DeleteDepartmentAsync(request.DepartmentId);
            return new()
            {
                Deleted = result,
                StatusCode = result ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound,
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while deleting the department {request.DepartmentId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
