using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Faculties.Responses;

namespace QCUniversidad.Api.Requests.Faculties.Models;

public class GetFacultiesRangeRequest : RequestBase<GetFacultiesRangeRequestResponse>
{
    public int From { get; set; }
    public int To { get; set; }
}
