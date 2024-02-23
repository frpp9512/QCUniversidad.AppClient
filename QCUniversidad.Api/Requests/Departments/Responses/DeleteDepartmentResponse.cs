using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record DeleteDepartmentResponse : RequestResponseBase
{
    public bool Deleted { get; set; }

    public override object? GetPayload() => Deleted;
}
