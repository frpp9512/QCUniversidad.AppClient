using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class ExistsCareerHandler(ICareersManager careersManager) : IRequestHandler<ExistsCareerRequest, ExistsCareerResponse>
{
    private readonly ICareersManager _careersManager = careersManager;

    public async Task<ExistsCareerResponse> Handle(ExistsCareerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _careersManager.ExistsCareerAsync(request.CareerId);
            return new() { CareerExists = result };
        }
        catch (Exception ex)
        {
            return new() { ErrorMessages = [ex.Message] };
        }
    }
}
