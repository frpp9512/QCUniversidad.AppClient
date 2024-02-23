using MediatR;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class GetDisciplineByIdRequest : IRequest<GetDisciplineByIdRequestResponse>
{
    public Guid DisciplineId { get; set; }
}
