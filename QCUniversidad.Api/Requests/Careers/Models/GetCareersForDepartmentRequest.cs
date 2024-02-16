using MediatR;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class GetCareersForDepartmentRequest : IRequest<GetCareersForDepartmentResponse>
{
    public required Guid DepartmentId { get; set; }
}
