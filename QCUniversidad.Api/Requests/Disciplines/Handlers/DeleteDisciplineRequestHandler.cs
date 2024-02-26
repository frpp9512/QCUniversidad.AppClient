using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Disciplines.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Handlers;

public class DeleteDisciplineRequestHandler(IDisciplinesManager disciplinesManager) : IRequestHandler<DeleteDisciplineRequest, DeleteDisciplineRequestResponse>
{
    private readonly IDisciplinesManager _disciplinesManager = disciplinesManager;

    public async Task<DeleteDisciplineRequestResponse> Handle(DeleteDisciplineRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _disciplinesManager.DeleteDisciplineAsync(request.DisciplineId);
            return new()
            {
                RequestId = request.RequestId,
                Deleted = result,
                StatusCode = result ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError,
                ErrorMessages = result ? [] : [$"Error while deleting the discipline {request.DisciplineId}"]
            };
        }
        catch (DisciplineNotFoundException)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"The discipline with id {request.DisciplineId} was not found."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while deleting the discipline with id {request.DisciplineId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
