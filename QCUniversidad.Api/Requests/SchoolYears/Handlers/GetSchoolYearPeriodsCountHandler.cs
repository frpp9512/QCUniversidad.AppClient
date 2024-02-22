using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.SchoolYears.Models;
using QCUniversidad.Api.Requests.SchoolYears.Responses;

namespace QCUniversidad.Api.Requests.SchoolYears.Handlers;

public class GetSchoolYearPeriodsCountHandler(IPeriodsManager periodsManager) : IRequestHandler<GetSchoolYearPeriodsCountRequest, GetSchoolYearPeriodsCountResponse>
{
    private readonly IPeriodsManager _periodsManager = periodsManager;

    public async Task<GetSchoolYearPeriodsCountResponse> Handle(GetSchoolYearPeriodsCountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            int count = await _periodsManager.GetSchoolYearPeriodsCountAsync(request.SchoolYearId);
            return new()
            {
                SchoolYearId = request.SchoolYearId,
                PeriodsCount = count
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [ ex.Message ]
            };
        }
    }
}
