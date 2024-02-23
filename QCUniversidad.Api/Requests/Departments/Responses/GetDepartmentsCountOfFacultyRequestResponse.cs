using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record GetDepartmentsCountOfFacultyRequestResponse : RequestResponseBase
{
    public Guid FacultyId { get; set; }
    public int Count { get; set; }

    public override object? GetPayload() => Count;
}
