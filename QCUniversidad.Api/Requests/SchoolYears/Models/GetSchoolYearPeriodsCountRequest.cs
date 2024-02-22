using MediatR;
using QCUniversidad.Api.Requests.SchoolYears.Responses;

namespace QCUniversidad.Api.Requests.SchoolYears.Models;

public class GetSchoolYearPeriodsCountRequest : IRequest<GetSchoolYearPeriodsCountResponse>
{
    public Guid SchoolYearId { get; set; }
}
