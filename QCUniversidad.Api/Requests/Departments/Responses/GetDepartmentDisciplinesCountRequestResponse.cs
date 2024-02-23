using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record GetDepartmentDisciplinesCountRequestResponse : RequestResponseBase
{
    public int Count { get; set; }

    public override object? GetPayload() => Count;
}
