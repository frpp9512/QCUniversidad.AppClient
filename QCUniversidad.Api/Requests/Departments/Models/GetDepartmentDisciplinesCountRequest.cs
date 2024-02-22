using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentDisciplinesCountRequest : IRequest<GetDepartmentDisciplinesCountResponse>
{
    public Guid DepartmentId { get; set; }
}
