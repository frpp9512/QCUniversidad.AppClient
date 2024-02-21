using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record ExistDepartmentResponse : ResponseBase
{
    public Guid DepartmentId { get; set; }
    public bool Exist { get; set; }

    public override object? GetPayload() => Exist;
}
