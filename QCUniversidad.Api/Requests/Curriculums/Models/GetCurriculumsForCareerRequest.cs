using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;

namespace QCUniversidad.Api.Requests.Curriculums.Models;

public class GetCurriculumsForCareerRequest : RequestBase<GetCurriculumsForCareerRequestResponse>
{
    public Guid CareerId { get; set; }
}
