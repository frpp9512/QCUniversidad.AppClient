using MediatR;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class ExistsCareerRequest : IRequest<ExistsCareerResponse>
{
    public required Guid CareerId { get; set; }
}
