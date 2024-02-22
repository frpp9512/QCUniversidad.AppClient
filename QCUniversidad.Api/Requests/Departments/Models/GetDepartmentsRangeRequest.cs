using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentsRangeRequest : IRequest<GetDepartmentsRangeResponse>
{
    public int From { get; set; }
    public int To { get; set; }
}
