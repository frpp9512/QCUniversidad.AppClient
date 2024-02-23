using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Disciplines.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Requests.Disciplines.Handlers;

public class GetDisciplineByNameRequestHandler(IDisciplinesManager disciplinesManager, IMapper mapper) : IRequestHandler<GetDisciplineByNameRequest, GetDisciplineByNameRequestResponse>
{
    private readonly IDisciplinesManager _disciplinesManager = disciplinesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetDisciplineByNameRequestResponse> Handle(GetDisciplineByNameRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.DisciplineName))
        {
            return new()
            {
                ErrorMessages = [$"Must provide a discipline name."],
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }

        try
        {
            DisciplineModel result = await _disciplinesManager.GetDisciplineAsync(request.DisciplineName);
            PopulatedDisciplineDto dto = _mapper.Map<PopulatedDisciplineDto>(result);
            dto.SubjectsCount = await _disciplinesManager.GetDisciplineSubjectsCountAsync(dto.Id);
            dto.TeachersCount = await _disciplinesManager.GetDisciplineTeachersCountAsync(dto.Id);
            return new()
            {
                DisciplineName = request.DisciplineName,
                Discipline = dto
            };
        }
        catch (DisciplineNotFoundException)
        {
            return new()
            {
                ErrorMessages = [$"The discipline with name {request.DisciplineName} was not found."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while fetching the discipline with name {request.DisciplineName}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
