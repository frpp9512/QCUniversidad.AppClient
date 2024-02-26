using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Faculties.Responses;

public record ExistFacultyRequestResponse : RequestResponseBase
{
    public Guid FacultyId { get; set; }
    public bool Exist { get; set; }

    public override object? GetPayload() => Exist;
}
