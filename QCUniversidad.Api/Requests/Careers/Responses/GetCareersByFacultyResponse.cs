using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Career;
using System.Security.Principal;

namespace QCUniversidad.Api.Requests.Careers.Responses;

public record GetCareersByFacultyResponse : ResponseBase
{
    public List<CareerDto>? FacultyCareers { get; set; }
}
