using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Curriculums.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;

namespace QCUniversidad.Api.Requests.Curriculums.Handlers;

public class GetCurriculumsCountHandler(ICurriculumsManager curriculumsManager) : IRequestHandler<GetCurriculumsCountRequest, GetCurriculumsCountResponse>
{
    private readonly ICurriculumsManager _curriculumsManager = curriculumsManager;

    public async Task<GetCurriculumsCountResponse> Handle(GetCurriculumsCountRequest request, CancellationToken cancellationToken)
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
