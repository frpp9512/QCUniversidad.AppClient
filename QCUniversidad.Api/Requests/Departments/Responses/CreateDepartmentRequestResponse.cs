using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record CreateDepartmentRequestResponse : CreatedRequestResponseBase<Guid, DepartmentDto> { }
