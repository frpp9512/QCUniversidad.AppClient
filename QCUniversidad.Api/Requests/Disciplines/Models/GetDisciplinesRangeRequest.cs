using MediatR;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class GetDisciplinesRangeRequest : IRequest<GetDisciplinesRangeRequestResponse>
{
    public int From { get; set; }
    public int To { get; set; }
}
