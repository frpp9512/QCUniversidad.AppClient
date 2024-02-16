using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class GetCareersForDepartmentHandler(ICareersManager careersManager, IMapper mapper) : IRequestHandler<GetCareersForDepartmentRequest, GetCareersForDepartmentResponse>
{
    private readonly ICareersManager _careersManager = careersManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCareersForDepartmentResponse> Handle(GetCareersForDepartmentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<CareerModel> careers = await _careersManager.GetCareersForDepartmentAsync(request.DepartmentId);
            List<CareerDto> dtos = careers.Select(_mapper.Map<CareerDto>).ToList();
            return new()
            {
                DepartmentCareers = dtos
            };
        }
        catch (DepartmentNotFoundException)
        {
            return new() { ErrorMessages = [$"The faculty with the id {request.DepartmentId} was not found."] };
        }
        catch (Exception ex)
        {
            return new() { ErrorMessages = [ ex.Message ] };
        }
    }
}
