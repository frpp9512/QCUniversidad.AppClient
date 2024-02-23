using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Curriculums.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;

namespace QCUniversidad.Api.Requests.Curriculums.Handlers;

public class ExistsCurriculumRequestHandler(ICurriculumsManager curriculumsManager) : IRequestHandler<ExistCurriculumRequest, ExistCurriculumRequestResponse>
{
    private readonly ICurriculumsManager _curriculumsManager = curriculumsManager;

    public async Task<ExistCurriculumRequestResponse> Handle(ExistCurriculumRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _curriculumsManager.ExistsCurriculumAsync(request.CurriculumId);
            return new()
            {
                ExistCurriculum = result,
                StatusCode = result ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error fetching the count of curriculums. Error messages: {ex.Message}"]
            };
        }
    }
}
