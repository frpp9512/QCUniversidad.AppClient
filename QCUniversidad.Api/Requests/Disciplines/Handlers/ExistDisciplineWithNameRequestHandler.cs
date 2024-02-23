using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Disciplines.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Handlers;

public class ExistDisciplineWithNameRequestHandler(IDisciplinesManager disciplinesManager) : IRequestHandler<ExistDisciplineWithNameRequest, ExistDisciplineWithNameRequestResponse>
{
    private readonly IDisciplinesManager _disciplinesManager = disciplinesManager;

    public async Task<ExistDisciplineWithNameRequestResponse> Handle(ExistDisciplineWithNameRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            return new()
            {
                ErrorMessages = ["Should provide a discipline name."],
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }

        try
        {
            bool result = await _disciplinesManager.ExistsDisciplineAsync(request.Name);
            return new()
            {
                Name = request.Name,
                Exist = result
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while determining if the discipline with name {request.Name}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
