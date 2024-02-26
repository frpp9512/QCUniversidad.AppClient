using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class GetCareersForFacultyRequest : RequestBase<GetCareersByFacultyRequestResponse>
{
    public required Guid FacultyId { get; set; }
}
