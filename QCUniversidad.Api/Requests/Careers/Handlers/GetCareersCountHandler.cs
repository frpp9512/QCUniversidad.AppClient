using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class GetCareersCountHandler(ICareersManager careersManager) : IRequestHandler<GetCareersCountRequest, GetCareersCountResponse>
{
    private readonly ICareersManager _careersManager = careersManager;

    public async Task<GetCareersCountResponse> Handle(GetCareersCountRequest request, CancellationToken cancellationToken)
    {
		try
		{
			var count = await _careersManager.GetCareersCountAsync();
			return new() { CareersCount = count };
		}
		catch (Exception ex)
		{
			return new() { ErrorMessages = [ ex.Message ] };
		}
    }
}
