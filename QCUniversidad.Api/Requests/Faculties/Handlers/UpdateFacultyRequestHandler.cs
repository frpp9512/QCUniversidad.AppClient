using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Faculties.Models;
using QCUniversidad.Api.Requests.Faculties.Responses;

namespace QCUniversidad.Api.Requests.Faculties.Handlers;

public class UpdateFacultyRequestHandler(IFacultiesManager facultiesManager, IMapper mapper) : IRequestHandler<UpdateFacultyRequest, UpdateFacultyRequestResponse>
{
    private readonly IFacultiesManager _facultiesManager = facultiesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateFacultyRequestResponse> Handle(UpdateFacultyRequest request, CancellationToken cancellationToken)
    {
        if (request.Faculty is null)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Must provide the faculty data."],
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }

        try
        {
            var result = await _facultiesManager.UpdateFacultyAsync(_mapper.Map<FacultyModel>(request.Faculty));
            return new()
            {
                RequestId = request.RequestId,
                Updated = result
            };
        }
        catch (FacultyNotFoundException)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"The faculty was not found."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while updating the faculty. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
