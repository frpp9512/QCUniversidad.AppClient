using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Curriculums.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;

namespace QCUniversidad.Api.Requests.Curriculums.Handlers;

public class DeleteCurriculumRequestHandler(ICurriculumsManager curriculumsManager) : IRequestHandler<DeleteCurriculumRequest, DeleteCurriculumRequestResponse>
{
    private readonly ICurriculumsManager _curriculumsManager = curriculumsManager;

    public async Task<DeleteCurriculumRequestResponse> Handle(DeleteCurriculumRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _curriculumsManager.DeleteCurriculumAsync(request.CurriculumId);
            return new()
            {
                Deleted = result,
                StatusCode = result ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError
            };
        }
        catch (CurriculumNotFoundException)
        {
            return new()
            {
                ErrorMessages = [$"The curriculum with id: {request.CurriculumId} was not found."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while deleting the curriculum: {request.CurriculumId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
