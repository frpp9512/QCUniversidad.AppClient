using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class GetCareersForDepartmentRequest : RequestBase<GetCareersForDepartmentRequestResponse>
{
    public required Guid DepartmentId { get; set; }
}
