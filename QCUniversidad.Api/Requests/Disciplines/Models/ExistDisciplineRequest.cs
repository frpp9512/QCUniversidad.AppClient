using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class ExistDisciplineRequest : RequestBase<ExistDisciplineRequestResponse>
{
    public Guid DisciplineId { get; set; }
}
