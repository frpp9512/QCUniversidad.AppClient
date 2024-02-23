using MediatR;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class DeleteDisciplineRequest : IRequest<DeleteDisciplineRequestResponse>
{
    public Guid DisciplineId { get; set; }
}
