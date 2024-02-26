using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class ExistsCareerRequest : RequestBase<ExistsCareerRequestResponse>
{
    public required Guid CareerId { get; set; }
}
