using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentByIdRequest : IRequest<GetDepartmentByIdResponse>
{
    public Guid DepartmentId { get; set; }
}
