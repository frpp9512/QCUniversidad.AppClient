using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Curriculums.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Requests.Curriculums.Handlers;

public class GetCurriculumsForCareerHandler(ICurriculumsManager curriculumsManager, IMapper mapper) : IRequestHandler<GetCurriculumsForCareerRequest, GetCurriculumsForCareerResponse>
{
    private readonly ICurriculumsManager _curriculumsManager = curriculumsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCurriculumsForCareerResponse> Handle(GetCurriculumsForCareerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<CurriculumModel> curriculums = await _curriculumsManager.GetCurriculumsForCareerAsync(request.CareerId);
            var dtos = curriculums.Select(_mapper.Map<CurriculumDto>).ToList();
            return new()
            {
                CareerCurriculums = dtos
            };
        }
        catch (CareerNotFoundException)
        {
            return new()
            {
                ErrorMessages = [$"The career {request.CareerId} do not exist."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while fetching the curriculums for the career: {request.CareerId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
