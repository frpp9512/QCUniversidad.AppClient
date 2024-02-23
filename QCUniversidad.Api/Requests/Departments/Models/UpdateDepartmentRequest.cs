using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class UpdateDepartmentRequest : IRequest<UpdateDepartmentRequestResponse>
{
    public EditDepartmentDto? DepartmentToUpdate { get; set; }
}
