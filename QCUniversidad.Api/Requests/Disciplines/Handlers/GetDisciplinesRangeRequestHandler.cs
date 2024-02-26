using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Disciplines.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Requests.Disciplines.Handlers;

public class GetDisciplinesRangeRequestHandler(IDisciplinesManager disciplinesManager, IMapper mapper) : IRequestHandler<GetDisciplinesRangeRequest, GetDisciplinesRangeRequestResponse>
{
    private readonly IDisciplinesManager _disciplinesManager = disciplinesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetDisciplinesRangeRequestResponse> Handle(GetDisciplinesRangeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<DisciplineModel> disciplines = await _disciplinesManager.GetDisciplinesAsync(request.From, request.To);
            List<PopulatedDisciplineDto> dtos = disciplines.Select(_mapper.Map<PopulatedDisciplineDto>).ToList();
            foreach (PopulatedDisciplineDto? dto in dtos)
            {
                dto.TeachersCount = await _disciplinesManager.GetDisciplineTeachersCountAsync(dto.Id);
                dto.SubjectsCount = await _disciplinesManager.GetDisciplineSubjectsCountAsync(dto.Id);
            }

            return new()
            {
                RequestId = request.RequestId,
                From = request.From,
                To = request.To,
                Disciplines = dtos
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while fetching the disciplines range from {request.From} to {request.To}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
