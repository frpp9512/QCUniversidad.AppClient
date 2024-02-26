using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class ExistDisciplineWithNameRequest : RequestBase<ExistDisciplineWithNameRequestResponse>
{
    public string? Name { get; set; }
}
