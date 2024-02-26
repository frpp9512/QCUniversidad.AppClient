using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Careers.Responses;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class CreateCareerRequest : RequestBase<CreateCareerRequestResponse>
{
    public required NewCareerDto NewCareerDto { get; set; }
}
