using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class GetCareersCountRequestHandler(ICareersManager careersManager) : IRequestHandler<GetCareersCountRequest, GetCareersCountRequestResponse>
{
    private readonly ICareersManager _careersManager = careersManager;

    public async Task<GetCareersCountRequestResponse> Handle(GetCareersCountRequest request, CancellationToken cancellationToken)
    {
		try
		{
			var count = await _careersManager.GetCareersCountAsync();
			return new() { CareersCount = count };
		}
		catch (Exception ex)
		{
			return new()
			{
				ErrorMessages = [ ex.Message ],
				StatusCode = System.Net.HttpStatusCode.InternalServerError
			};
		}
    }
}
