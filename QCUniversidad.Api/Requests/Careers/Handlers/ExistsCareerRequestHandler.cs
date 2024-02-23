using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class ExistsCareerRequestHandler(ICareersManager careersManager) : IRequestHandler<ExistsCareerRequest, ExistsCareerRequestResponse>
{
    private readonly ICareersManager _careersManager = careersManager;

    public async Task<ExistsCareerRequestResponse> Handle(ExistsCareerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _careersManager.ExistsCareerAsync(request.CareerId);
            return new() { CareerExists = result };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [ex.Message],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
