using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Faculties.Models;
using QCUniversidad.Api.Requests.Faculties.Responses;

namespace QCUniversidad.Api.Requests.Faculties.Handlers;

public class ExistFacultyRequestHandler(IFacultiesManager facultiesManager) : IRequestHandler<ExistFacultyRequest, ExistFacultyRequestResponse>
{
    private readonly IFacultiesManager _facultiesManager = facultiesManager;

    public async Task<ExistFacultyRequestResponse> Handle(ExistFacultyRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _facultiesManager.ExistFacultyAsync(request.FacultyId);
            return new()
            {
                RequestId = request.RequestId,
                Exist = result,
                StatusCode = result ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while determining if faculty {request.FacultyId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
