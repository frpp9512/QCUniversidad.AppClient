using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Disciplines.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Requests.Disciplines.Handlers;

public class GetDisciplineByIdRequestHandler(IDisciplinesManager disciplinesManager, IMapper mapper) : IRequestHandler<GetDisciplineByIdRequest, GetDisciplineByIdRequestResponse>
{
    private readonly IDisciplinesManager _disciplinesManager = disciplinesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetDisciplineByIdRequestResponse> Handle(GetDisciplineByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            DisciplineModel result = await _disciplinesManager.GetDisciplineAsync(request.DisciplineId);
            PopulatedDisciplineDto dto = _mapper.Map<PopulatedDisciplineDto>(result);
            dto.SubjectsCount = await _disciplinesManager.GetDisciplineSubjectsCountAsync(dto.Id);
            dto.TeachersCount = await _disciplinesManager.GetDisciplineTeachersCountAsync(dto.Id);
            return new()
            {
                Discipline = dto,
                DisciplineId = dto.Id
            };
        }
        catch (DisciplineNotFoundException)
        {
            return new()
            {
                ErrorMessages = [$"The discipline with id {request.DisciplineId} do not exist."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while fetching the discipline with id {request.DisciplineId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
