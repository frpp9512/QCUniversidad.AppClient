using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Faculties.Models;
using QCUniversidad.Api.Requests.Faculties.Responses;

namespace QCUniversidad.Api.Requests.Faculties.Handlers;

public class DeleteFacultyRequestHandler(IFacultiesManager facultiesManager) : IRequestHandler<DeleteFacultyRequest, DeleteFacultyRequestResponse>
{
    private readonly IFacultiesManager _facultiesManager = facultiesManager;

    public async Task<DeleteFacultyRequestResponse> Handle(DeleteFacultyRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _facultiesManager.DeleteFacultyAsync(request.FacultyId);
            return new()
            {
                RequestId = request.RequestId,
                FacultyId = request.FacultyId,
                Deleted = result,
                StatusCode = result ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError,
                ErrorMessages = [$"Error deleting the faculty with id {request.FacultyId}"]
            };
        }
        catch (FacultyNotFoundException)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"The faculty with id {request.FacultyId} do not exist."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while deleting the faculty with id {request.RequestId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
