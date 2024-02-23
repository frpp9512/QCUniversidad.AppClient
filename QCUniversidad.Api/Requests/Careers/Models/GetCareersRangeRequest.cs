using MediatR;
using QCUniversidad.Api.Requests.Careers.Responses;

namespace QCUniversidad.Api.Requests.Careers.Models;

public class GetCareersRangeRequest : IRequest<GetCareersRangeRequestResponse>
{
    public int From { get; set; }
    public int To { get; set; }
}
