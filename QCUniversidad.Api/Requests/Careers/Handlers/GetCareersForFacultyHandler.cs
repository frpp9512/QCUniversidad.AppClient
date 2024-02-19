using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Careers.Models;
using QCUniversidad.Api.Requests.Careers.Responses;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Handlers;

public class GetCareersForFacultyHandler(ICareersManager careersManager, IMapper mapper) : IRequestHandler<GetCareersForFacultyRequest, GetCareersByFacultyResponse>
{
    private readonly ICareersManager _careersManager = careersManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCareersByFacultyResponse> Handle(GetCareersForFacultyRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<CareerModel> careers = await _careersManager.GetCareersAsync(request.FacultyId);
            List<CareerDto> dtos = careers.Select(_mapper.Map<CareerDto>).ToList();
            return new()
            {
                FacultyCareers = dtos
            };
        }
        catch (FacultyNotFoundException)
        {
            return new()
            {
                ErrorMessages = [$"The faculty with the id {request.FacultyId} was not found."],
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
