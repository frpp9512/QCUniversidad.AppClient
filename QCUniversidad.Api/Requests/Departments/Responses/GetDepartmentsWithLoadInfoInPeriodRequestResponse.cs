using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record GetDepartmentsWithLoadInfoInPeriodRequestResponse : RequestResponseBase
{
    public Guid PeriodId { get; set; }
    public List<DepartmentDto>? Departments { get; set; }

    public override object? GetPayload() => Departments;
}
