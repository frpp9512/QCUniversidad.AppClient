using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Faculties.Models;
using QCUniversidad.Api.Requests.Faculties.Responses;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Requests.Faculties.Handlers;

public class CreateFacultyRequestHandler(IFacultiesManager facultiesManager, IMapper mapper) : IRequestHandler<CreateFacultyRequest, CreateFacultyRequestResponse>
{
    private readonly IFacultiesManager _facultiesManager = facultiesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateFacultyRequestResponse> Handle(CreateFacultyRequest request, CancellationToken cancellationToken)
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

        var result = await _facultiesManager.CreateFacultyAsync(_mapper.Map<FacultyModel>(request.Faculty));
        return new()
        {
            RequestId = request.RequestId,
            CreatedEntity = _mapper.Map<FacultyDto>(result),
            CreatedId = result?.Id ?? Guid.Empty,
            ApiEntityEndpointAction = "GetById",
            StatusCode = result is null ? System.Net.HttpStatusCode.InternalServerError : System.Net.HttpStatusCode.OK,
            ErrorMessages = result is null ? [$"Error while creating the faculty."] : []
        };
    }
}
