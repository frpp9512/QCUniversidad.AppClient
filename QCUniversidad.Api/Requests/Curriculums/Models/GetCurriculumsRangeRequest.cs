using MediatR;
using QCUniversidad.Api.Requests.Curriculums.Responses;

namespace QCUniversidad.Api.Requests.Curriculums.Models;

public class GetCurriculumsRangeRequest : IRequest<GetCurriculumsRangeRequestResponse>
{
    public int From { get; set; }
    public int To { get; set; }
}
