using MediatR;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class ExistDisciplineRequest : IRequest<ExistDisciplineRequestResponse>
{
    public Guid DisciplineId { get; set; }
}
