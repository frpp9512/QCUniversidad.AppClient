using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class DeleteDepartmentRequest : IRequest<DeleteDepartmentRequestResponse>
{
    public Guid DepartmentId { get; set; }
}
