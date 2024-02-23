using MediatR;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class GetCareersForFacultyRequest : IRequest<GetCareersByFacultyRequestResponse>
{
    public required Guid FacultyId { get; set; }
}
