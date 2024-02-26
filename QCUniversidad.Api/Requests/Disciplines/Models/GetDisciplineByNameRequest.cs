using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class GetDisciplineByNameRequest : RequestBase<GetDisciplineByNameRequestResponse>
{
    public string? DisciplineName { get; set; }
}
