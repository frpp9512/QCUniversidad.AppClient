using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record CreateDepartmentResponse : CreatedResponseBase<Guid, DepartmentDto> { }
