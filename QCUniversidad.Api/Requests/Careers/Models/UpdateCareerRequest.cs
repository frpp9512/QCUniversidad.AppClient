using MediatR;
using QCUniversidad.Api.Requests.Careers.Responses;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class UpdateCareerRequest : IRequest<UpdateCareerRequestResponse>
{
    public required EditCareerDto Career { get; set; }
}
