using MediatR;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class GetCareersForFacultyRequest : IRequest<GetCareersByFacultyResponse>
{
    public required Guid FacultyId { get; set; }
}
