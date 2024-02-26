using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class DeleteDisciplineRequest : RequestBase<DeleteDisciplineRequestResponse>
{
    public Guid DisciplineId { get; set; }
}
