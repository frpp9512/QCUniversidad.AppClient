using MediatR;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class GetCareerByIdRequest : IRequest<GetCareerByIdRequestResponse>
{
    public required Guid CareerId { get; set; }
}
