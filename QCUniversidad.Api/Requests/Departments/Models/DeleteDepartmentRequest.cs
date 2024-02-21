using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class DeleteDepartmentRequest : IRequest<DeleteDepartmentResponse>
{
    public Guid DepartmentId { get; set; }
}
