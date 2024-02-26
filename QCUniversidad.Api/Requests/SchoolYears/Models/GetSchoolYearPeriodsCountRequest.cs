using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.SchoolYears.Responses;

namespace QCUniversidad.Api.Requests.SchoolYears.Models;

public class GetSchoolYearPeriodsCountRequest : RequestBase<GetSchoolYearPeriodsCountResponse>
{
    public Guid SchoolYearId { get; set; }
}
