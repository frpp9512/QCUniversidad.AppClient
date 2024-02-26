using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentsRangeRequest : RequestBase<GetDepartmentsRangeRequestResponse>
{
    public int From { get; set; }
    public int To { get; set; }
}
