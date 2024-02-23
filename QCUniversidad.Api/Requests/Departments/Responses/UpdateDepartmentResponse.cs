using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record UpdateDepartmentResponse : RequestResponseBase
{
    public bool Updated { get; set; }

    public override object? GetPayload() => Updated;
}
