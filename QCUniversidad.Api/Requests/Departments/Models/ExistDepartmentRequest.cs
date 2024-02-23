using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class ExistDepartmentRequest : IRequest<ExistDepartmentRequestResponse>
{
    public Guid DepartmentId { get; set; }
}
