using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Disciplines.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Handlers;

public class UpdateDisciplineRequestHandler(IDisciplinesManager disciplinesManager, IMapper mapper) : IRequestHandler<UpdateDisciplineRequest, UpdateDisciplineRequestResponse>
{
    private readonly IDisciplinesManager _disciplinesManager = disciplinesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateDisciplineRequestResponse> Handle(UpdateDisciplineRequest request, CancellationToken cancellationToken)
    {
        if (request.DisciplineToUpdate is null)
        {
            return new()
            {
                ErrorMessages = [$"Must provide the discipline data to update."],
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }

        try
        {
            bool result = await _disciplinesManager.UpdateDisciplineAsync(_mapper.Map<DisciplineModel>(request.DisciplineToUpdate));
            return new()
            {
                Updated = result,
                StatusCode = result ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError,
                ErrorMessages = result ? [] : [$"Error while updating a discipline."]
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while updating a discipline. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
