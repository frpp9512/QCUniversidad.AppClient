using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class DeleteCareerHandler(ICareersManager careersManager) : IRequestHandler<DeleteCareerRequest, DeleteCareerResponse>
{
    private readonly ICareersManager _careersManager = careersManager;

    public async Task<DeleteCareerResponse> Handle(DeleteCareerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _careersManager.DeleteCareerAsync(request.CareerId);
            return new() { CareerId = request.CareerId, Deleted = result };
        }
        catch (CareerNotFoundException)
        {
            return new() { ErrorMessages = [$"The career with id {request.CareerId} was not found in database."] };
        }
        catch (Exception ex)
        {
            return new() { ErrorMessages = [ex.Message] };
        }
    }
}
