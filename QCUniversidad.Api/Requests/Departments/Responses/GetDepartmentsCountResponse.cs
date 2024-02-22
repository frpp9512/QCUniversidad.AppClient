using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record GetDepartmentsCountResponse : ResponseBase
{
    public int Count { get; set; }

    public override object? GetPayload() => Count;
}
