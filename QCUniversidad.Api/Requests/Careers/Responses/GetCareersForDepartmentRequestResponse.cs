using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Responses;

public record GetCareersForDepartmentRequestResponse : RequestResponseBase
{
    public List<CareerDto>? DepartmentCareers { get; set; }

    public override object? GetPayload() => DepartmentCareers;
}
