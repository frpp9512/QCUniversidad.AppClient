using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Faculties.Responses;

public record DeleteFacultyRequestResponse : RequestResponseBase
{
    public Guid FacultyId { get; set; }
    public bool Deleted { get; set; }

    public override object? GetPayload() => Deleted;
}
