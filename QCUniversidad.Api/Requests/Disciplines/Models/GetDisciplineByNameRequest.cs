using MediatR;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class GetDisciplineByNameRequest : IRequest<GetDisciplineByNameRequestResponse>
{
    public string? DisciplineName { get; set; }
}
