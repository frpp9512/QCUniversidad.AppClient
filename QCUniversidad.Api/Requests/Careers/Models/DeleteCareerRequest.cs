using MediatR;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class DeleteCareerRequest : IRequest<DeleteCareerResponse>
{
    public Guid CareerId { get; set; }
}
