using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record GetDepartmentsCountRequestResponse : RequestResponseBase
{
    public int Count { get; set; }

    public override object? GetPayload() => Count;
}
