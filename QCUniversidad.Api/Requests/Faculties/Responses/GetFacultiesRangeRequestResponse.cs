using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Requests.Faculties.Responses;

public record GetFacultiesRangeRequestResponse : RequestResponseBase
{
    public int From { get; set; }
    public int To { get; set; }
    public List<FacultyDto>? Faculties { get; set; }

    public override object? GetPayload() => Faculties;
}
