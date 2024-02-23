using MediatR;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class ExistDisciplineWithNameRequest : IRequest<ExistDisciplineWithNameRequestResponse>
{
    public string? Name { get; set; }
}
