using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Requests.Faculties.Responses;

public record GetFacultyByIdRequestResponse : RequestResponseBase
{
    public Guid FacultyId { get; set; }
    public FacultyDto? Faculty { get; set; }

    public override object? GetPayload() => Faculty;
}
