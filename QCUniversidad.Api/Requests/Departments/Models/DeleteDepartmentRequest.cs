using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class DeleteDepartmentRequest : RequestBase<DeleteDepartmentRequestResponse>
{
    public Guid DepartmentId { get; set; }
}
