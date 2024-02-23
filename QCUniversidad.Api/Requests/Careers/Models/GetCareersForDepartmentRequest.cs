using MediatR;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class GetCareersForDepartmentRequest : IRequest<GetCareersForDepartmentRequestResponse>
{
    public required Guid DepartmentId { get; set; }
}
