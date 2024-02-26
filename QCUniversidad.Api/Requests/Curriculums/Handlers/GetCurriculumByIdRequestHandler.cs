using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Curriculums.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;
using QCUniversidad.Api.Shared.Dtos.Curriculum;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Requests.Curriculums.Handlers;

public class GetCurriculumByIdRequestHandler(ICurriculumsManager curriculumsManager, IMapper mapper) : IRequestHandler<GetCurriculumByIdRequest, GetCurriculumByIdRequestResponse>
{
    private readonly ICurriculumsManager _curriculumsManager = curriculumsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCurriculumByIdRequestResponse> Handle(GetCurriculumByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            CurriculumModel result = await _curriculumsManager.GetCurriculumAsync(request.CurriculumId);
            CurriculumDto dto = _mapper.Map<CurriculumDto>(result);
            dto.CurriculumDisciplines = result.CurriculumDisciplines
                                           .Select(cs => _mapper.Map<SimpleDisciplineDto>(cs.Discipline))
                                           .ToList();
            return new()
            {
                RequestId = request.RequestId,
                CurriculumId = request.CurriculumId,
                Curriculum = dto
            };
        }
        catch (CurriculumNotFoundException)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"The curriculum with id {request.CurriculumId} was not found."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while fetching the curriculum with id {request.CurriculumId}. Error message: {ex.Message}"]
            };
        }
    }
}
