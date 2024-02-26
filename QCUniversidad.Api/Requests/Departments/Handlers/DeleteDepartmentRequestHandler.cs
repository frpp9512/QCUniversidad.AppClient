using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class DeleteDepartmentRequestHandler(IDepartmentsManager departmentsManager) : IRequestHandler<DeleteDepartmentRequest, DeleteDepartmentRequestResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;

    public async Task<DeleteDepartmentRequestResponse> Handle(DeleteDepartmentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _departmentsManager.DeleteDepartmentAsync(request.DepartmentId);
            return new()
            {
                RequestId = request.RequestId,
                Deleted = result,
                StatusCode = result ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound,
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while deleting the department {request.DepartmentId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
