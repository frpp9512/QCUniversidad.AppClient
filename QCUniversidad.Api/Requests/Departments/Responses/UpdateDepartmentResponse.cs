using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record UpdateDepartmentResponse : ResponseBase
{
    public bool Updated { get; set; }

    public override object? GetPayload() => Updated;
}
