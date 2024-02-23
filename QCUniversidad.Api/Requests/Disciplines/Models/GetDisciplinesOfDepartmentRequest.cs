using MediatR;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class GetDisciplinesOfDepartmentRequest : IRequest<GetDisciplinesOfDepartmentRequestResponse>
{
    public Guid DepartmentId { get; set; }
}
