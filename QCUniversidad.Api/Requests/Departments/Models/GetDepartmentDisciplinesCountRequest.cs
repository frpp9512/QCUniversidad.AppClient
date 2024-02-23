using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentDisciplinesCountRequest : IRequest<GetDepartmentDisciplinesCountRequestResponse>
{
    public Guid DepartmentId { get; set; }
}
