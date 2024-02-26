using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class GetCareerByIdRequest : RequestBase<GetCareerByIdRequestResponse>
{
    public required Guid CareerId { get; set; }
}
