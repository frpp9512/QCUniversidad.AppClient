using MediatR;
using QCUniversidad.Api.Requests.Careers.Responses;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class CreateCareerRequest : IRequest<CreateCareerResponse>
{
    public required NewCareerDto NewCareerDto { get; set; }
}
