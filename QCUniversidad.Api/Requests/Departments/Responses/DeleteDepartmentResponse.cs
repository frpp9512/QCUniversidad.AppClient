using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record DeleteDepartmentResponse : ResponseBase
{
    public bool Deleted { get; set; }

    public override object? GetPayload() => Deleted;
}
