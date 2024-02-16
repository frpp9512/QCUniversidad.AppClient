using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Career;
using System.Security.Principal;

namespace QCUniversidad.Api.Requests.Careers.Responses;

public record GetCareersForDepartmentResponse : ResponseBase
{
    public List<CareerDto>? DepartmentCareers { get; set; }
}
