using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class GetCareersRangeRequestHandler(ICareersManager careersManager, IMapper mapper) : IRequestHandler<GetCareersRangeRequest, GetCareersRangeRequestResponse>
{
    private readonly ICareersManager _careersManager = careersManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCareersRangeRequestResponse> Handle(GetCareersRangeRequest request, CancellationToken cancellationToken)
    {
		try
		{
			IList<CareerModel> careers = await _careersManager.GetCareersAsync(request.From, request.To);
			List<CareerDto> dtos = careers.Select(_mapper.Map<CareerDto>).ToList();
			return new()
			{
				Careers = dtos,
				From = request.From,
				To = request.To,
			};
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
