using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record UpdateDepartmentRequestResponse : RequestResponseBase
{
    public bool Updated { get; set; }

    public override object? GetPayload() => Updated;
}
