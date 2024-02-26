using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Disciplines.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Requests.Disciplines.Handlers;

public class GetDisciplinesOfDepartmentRequestHandler(IDisciplinesManager disciplinesManager, IMapper mapper) : IRequestHandler<GetDisciplinesOfDepartmentRequest, GetDisciplinesOfDepartmentRequestResponse>
{
    private readonly IDisciplinesManager _disciplinesManager = disciplinesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetDisciplinesOfDepartmentRequestResponse> Handle(GetDisciplinesOfDepartmentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<DisciplineModel> disciplines = await _disciplinesManager.GetDisciplinesAsync(request.DepartmentId);
            List<PopulatedDisciplineDto> dtos = disciplines.Select(_mapper.Map<PopulatedDisciplineDto>).ToList();
            foreach (PopulatedDisciplineDto? dto in dtos)
            {
                dto.TeachersCount = await _disciplinesManager.GetDisciplineTeachersCountAsync(dto.Id);
                dto.SubjectsCount = await _disciplinesManager.GetDisciplineSubjectsCountAsync(dto.Id);
            }

            return new()
            {
                RequestId = request.RequestId,
                DepartmentId = request.DepartmentId,
                Disciplines = dtos
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while fetching the disciplines of department {request.DepartmentId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
