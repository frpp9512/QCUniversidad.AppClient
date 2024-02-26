using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Statistics.Responses;

namespace QCUniversidad.Api.Requests.Statistics.Models;

public class GetDepartmentStatisticsRequest : RequestBase<GetDepartmentStatisticsResponse>
{
    public Guid DepartmentId { get; set; }
    public Guid PeriodId { get; set; }
}
