using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class CreateDepartmentRequest : IRequest<CreateDepartmentResponse>
{
    public NewDepartmentDto? Department { get; set; }
}
