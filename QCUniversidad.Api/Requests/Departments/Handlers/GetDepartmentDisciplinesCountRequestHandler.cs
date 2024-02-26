using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class GetDepartmentDisciplinesCountRequestHandler(IDepartmentsManager departmentsManager) : IRequestHandler<GetDepartmentDisciplinesCountRequest, GetDepartmentDisciplinesCountRequestResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;

    public async Task<GetDepartmentDisciplinesCountRequestResponse> Handle(GetDepartmentDisciplinesCountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            int count = await _departmentsManager.GetDepartmentsCountAsync(request.DepartmentId);
            return new()
            {
                RequestId = request.RequestId,
                Count = count
            };
        }
        catch (DepartmentNotFoundException)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"The department with id {request.DepartmentId} was not found."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while fetching the count of the disciplines of the department {request.DepartmentId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
