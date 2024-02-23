using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Disciplines.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Handlers;

public class ExistDisciplineRequestHandler(IDisciplinesManager disciplinesManager) : IRequestHandler<ExistDisciplineRequest, ExistDisciplineRequestResponse>
{
    private readonly IDisciplinesManager _disciplinesManager = disciplinesManager;

    public async Task<ExistDisciplineRequestResponse> Handle(ExistDisciplineRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _disciplinesManager.ExistsDisciplineAsync(request.DisciplineId);
            return new()
            {
                DisciplineId = request.DisciplineId,
                Exist = result
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while determining if the discipline {request.DisciplineId} exists. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
