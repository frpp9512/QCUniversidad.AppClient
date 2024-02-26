using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class CreateCareerRequestHandler(ICareersManager careersManager, IMapper mapper) : IRequestHandler<CreateCareerRequest, CreateCareerRequestResponse>
{
    private readonly ICareersManager _careersManager = careersManager;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateCareerRequestResponse> Handle(CreateCareerRequest request, CancellationToken cancellationToken)
    {
        CareerModel model = _mapper.Map<CareerModel>(request.NewCareerDto);
        try
        {
            var createdCareer = await _careersManager.CreateCareerAsync(model);
            var careerDto = _mapper.Map<CareerDto>(createdCareer);
            return new CreateCareerRequestResponse
            {
                RequestId = request.RequestId,
                CreatedEntity = careerDto,
                CreatedId = createdCareer.Id,
                ApiEntityEndpointAction = "GetById"
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [ ex.Message ],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
