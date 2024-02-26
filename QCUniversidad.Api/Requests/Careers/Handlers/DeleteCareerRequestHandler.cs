using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class DeleteCareerRequestHandler(ICareersManager careersManager) : IRequestHandler<DeleteCareerRequest, DeleteCareerRequestResponse>
{
    private readonly ICareersManager _careersManager = careersManager;

    public async Task<DeleteCareerRequestResponse> Handle(DeleteCareerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _careersManager.DeleteCareerAsync(request.CareerId);
            return new()
            {
                RequestId = request.RequestId,
                CareerId = request.CareerId,
                Deleted = result
            };
        }
        catch (CareerNotFoundException)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"The career with id {request.CareerId} was not found in database."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [ex.Message],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
