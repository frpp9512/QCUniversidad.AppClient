using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Departments.Responses;
using QCUniversidad.Api.Requests.Disciplines.Models;

namespace QCUniversidad.Api.Requests.Disciplines.Handlers;

public class GetDisciplinesCountRequestHandler(IDisciplinesManager disciplinesManager) : IRequestHandler<GetDisciplinesCountRequest, GetDepartmentDisciplinesCountRequestResponse>
{
    private readonly IDisciplinesManager _disciplinesManager = disciplinesManager;

    public async Task<GetDepartmentDisciplinesCountRequestResponse> Handle(GetDisciplinesCountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            int count = await _disciplinesManager.GetDisciplinesCountAsync();
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
                ErrorMessages = [$"Error while fetching the disciplines count. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
