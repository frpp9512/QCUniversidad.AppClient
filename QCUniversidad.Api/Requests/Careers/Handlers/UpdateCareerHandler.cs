using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class UpdateCareerHandler(ICareersManager careersManager, IMapper mapper) : IRequestHandler<UpdateCareerRequest, UpdateCareerResponse>
{
    private readonly ICareersManager _careersManager = careersManager;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateCareerResponse> Handle(UpdateCareerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            CareerModel model = _mapper.Map<CareerModel>(request.Career);
            var updated = await _careersManager.UpdateCareerAsync(model);
            return new() { CareerUpdated = _mapper.Map<CareerDto>(updated) };
        }
        catch (CareerNotFoundException)
        {
            return new()
            {
                ErrorMessages = [$"The career with id {request.Career.Id} was not found in database."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
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
