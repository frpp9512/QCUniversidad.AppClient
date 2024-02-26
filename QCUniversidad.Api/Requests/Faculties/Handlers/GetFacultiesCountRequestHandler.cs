using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Faculties.Models;
using QCUniversidad.Api.Requests.Faculties.Responses;

namespace QCUniversidad.Api.Requests.Faculties.Handlers;

public class GetFacultiesCountRequestHandler(IFacultiesManager facultiesManager) : IRequestHandler<GetFacultiesCountRequest, GetFacultiesCountRequestResponse>
{
    private readonly IFacultiesManager _facultiesManager = facultiesManager;

    public async Task<GetFacultiesCountRequestResponse> Handle(GetFacultiesCountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var total = await _facultiesManager.GetFacultiesTotalAsync();
            return new()
            {
                RequestId = request.RequestId,
                Count = total
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error fetching the faculties count. Error message: {ex.Message}"]
            };
        }
    }
}
