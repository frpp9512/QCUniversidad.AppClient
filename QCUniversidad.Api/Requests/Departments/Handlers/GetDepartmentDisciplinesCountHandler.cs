using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class GetDepartmentDisciplinesCountHandler(IDepartmentsManager departmentsManager) : IRequestHandler<GetDepartmentDisciplinesCountRequest, GetDepartmentDisciplinesCountResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;

    public async Task<GetDepartmentDisciplinesCountResponse> Handle(GetDepartmentDisciplinesCountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            int count = await _departmentsManager.GetDepartmentsCountAsync(request.DepartmentId);
            return new()
            {
                Count = count
            };
        }
        catch (DepartmentNotFoundException)
        {
            return new()
            {
                ErrorMessages = [$"The department with id {request.DepartmentId} was not found."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while fetching the count of the disciplines of the department {request.DepartmentId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
