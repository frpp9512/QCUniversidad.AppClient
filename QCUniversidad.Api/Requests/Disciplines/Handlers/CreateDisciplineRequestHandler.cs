using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Disciplines.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Requests.Disciplines.Handlers;

public class CreateDisciplineRequestHandler(IDisciplinesManager disciplinesManager, IMapper mapper) : IRequestHandler<CreateDisciplineRequest, CreateDisciplineRequestResponse>
{
    private readonly IDisciplinesManager _disciplinesManager = disciplinesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateDisciplineRequestResponse> Handle(CreateDisciplineRequest request, CancellationToken cancellationToken)
    {
        if (request.NewDiscipline is null)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Must provide the discipline data."],
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }

        try
        {
            var result = await _disciplinesManager.CreateDisciplineAsync(_mapper.Map<DisciplineModel>(request.NewDiscipline));
            return new()
            {
                RequestId = request.RequestId,
                CreatedEntity = _mapper.Map<SimpleDisciplineDto>(result),
                ApiEntityEndpointAction = "GetById",
                CreatedId = result?.Id ?? Guid.Empty,
                StatusCode = result is null ? System.Net.HttpStatusCode.InternalServerError : System.Net.HttpStatusCode.Created,
                ErrorMessages = result is null ? [$"Error while creating the discipline."] : []
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while trying to add the discipline. Error message: {ex.Message}"]
            };
        }
    }
}
