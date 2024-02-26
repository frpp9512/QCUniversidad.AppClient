using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class GetCareerByIdRequestHandler(ICareersManager careersManager, IMapper mapper) : IRequestHandler<GetCareerByIdRequest, GetCareerByIdRequestResponse>
{
    private readonly ICareersManager _careersManager = careersManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCareerByIdRequestResponse> Handle(GetCareerByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            CareerModel career = await _careersManager.GetCareerAsync(request.CareerId);
            CareerDto dto = _mapper.Map<CareerDto>(career);
            return new()
            {
                RequestId = request.RequestId,
                Career = dto
            };
        }
        catch (CareerNotFoundException)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"The career with id {request.CareerId} was not found."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [ ex.Message ],
                StatusCode= System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
