using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Curriculums.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Requests.Curriculums.Handlers;

public class GetCurriculumsForCareerRequestHandler(ICurriculumsManager curriculumsManager, IMapper mapper) : IRequestHandler<GetCurriculumsForCareerRequest, GetCurriculumsForCareerRequestResponse>
{
    private readonly ICurriculumsManager _curriculumsManager = curriculumsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCurriculumsForCareerRequestResponse> Handle(GetCurriculumsForCareerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<CurriculumModel> curriculums = await _curriculumsManager.GetCurriculumsForCareerAsync(request.CareerId);
            var dtos = curriculums.Select(_mapper.Map<CurriculumDto>).ToList();
            return new()
            {
                RequestId = request.RequestId,
                CareerCurriculums = dtos
            };
        }
        catch (CareerNotFoundException)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"The career {request.CareerId} do not exist."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while fetching the curriculums for the career: {request.CareerId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
