using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Responses;

public record ExistsCareerResponse : ResponseBase
{
    public bool CareerExists { get; set; }
}
