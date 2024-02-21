using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record GetDepartmentsCountOfFacultyResponse : ResponseBase
{
    public Guid FacultyId { get; set; }
    public int Count { get; set; }

    public override object? GetPayload() => Count;
}
