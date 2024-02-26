using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class GetCareersForDepartmentRequestHandler(ICareersManager careersManager, IMapper mapper) : IRequestHandler<GetCareersForDepartmentRequest, GetCareersForDepartmentRequestResponse>
{
    private readonly ICareersManager _careersManager = careersManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCareersForDepartmentRequestResponse> Handle(GetCareersForDepartmentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<CareerModel> careers = await _careersManager.GetCareersForDepartmentAsync(request.DepartmentId);
            List<CareerDto> dtos = careers.Select(_mapper.Map<CareerDto>).ToList();
            return new()
            {
                RequestId = request.RequestId,
                DepartmentCareers = dtos
            };
        }
        catch (DepartmentNotFoundException)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"The faculty with the id {request.DepartmentId} was not found."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [ex.Message],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
