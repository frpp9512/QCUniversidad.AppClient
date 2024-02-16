using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class GetCareerByIdHandler(ICareersManager careersManager, IMapper mapper) : IRequestHandler<GetCareerByIdRequest, GetCareerByIdResponse>
{
    private readonly ICareersManager _careersManager = careersManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCareerByIdResponse> Handle(GetCareerByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            CareerModel career = await _careersManager.GetCareerAsync(request.CareerId);
            CareerDto dto = _mapper.Map<CareerDto>(career);
            return new() { Career = dto };
        }
        catch (CareerNotFoundException)
        {
            return new() { ErrorMessages = [$"The career with id {request.CareerId} was not found."] };
        }
        catch (Exception ex)
        {
            return new() { ErrorMessages = [ ex.Message ] };
        }
    }
}
