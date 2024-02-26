using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class DeleteCareerRequest : RequestBase<DeleteCareerRequestResponse>
{
    public Guid CareerId { get; set; }
}
