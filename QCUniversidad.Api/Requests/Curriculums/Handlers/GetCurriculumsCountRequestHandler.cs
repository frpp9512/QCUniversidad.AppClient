using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Curriculums.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;

namespace QCUniversidad.Api.Requests.Curriculums.Handlers;

public class GetCurriculumsCountRequestHandler(ICurriculumsManager curriculumsManager) : IRequestHandler<GetCurriculumsCountRequest, GetCurriculumsCountRequestResponse>
{
    private readonly ICurriculumsManager _curriculumsManager = curriculumsManager;

    public async Task<GetCurriculumsCountRequestResponse> Handle(GetCurriculumsCountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            int count = await _curriculumsManager.GetCurriculumsCountAsync();
            return new() { CurriculumsCount = count };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error fetching the count of the curriculums. Error message: {ex.Message}"]
            };
        }
    }
}
