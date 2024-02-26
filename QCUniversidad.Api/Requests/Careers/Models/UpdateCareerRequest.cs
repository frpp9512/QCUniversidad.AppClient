using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Careers.Responses;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class UpdateCareerRequest : RequestBase<UpdateCareerRequestResponse>
{
    public required EditCareerDto Career { get; set; }
}
