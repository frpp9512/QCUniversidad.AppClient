using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class GetDepartmentsCountOfFacultyHandler(IDepartmentsManager departmentsManager) : IRequestHandler<GetDepartmentsCountOfFacultyRequest, GetDepartmentsCountOfFacultyRequestResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;

    public async Task<GetDepartmentsCountOfFacultyRequestResponse> Handle(GetDepartmentsCountOfFacultyRequest request, CancellationToken cancellationToken)
    {
        try
        {
            int count = await _departmentsManager.GetDepartmentsCountAsync(request.FacultyId);
            return new()
            {
                Count = count
            };
        }
        catch (FacultyNotFoundException)
        {
            return new()
            {
                ErrorMessages = [$"The faculty with id {request.FacultyId} was not found."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while fetching the departments count the faculty {request.FacultyId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
