using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class GetDisciplineByIdRequest : RequestBase<GetDisciplineByIdRequestResponse>
{
    public Guid DisciplineId { get; set; }
}
