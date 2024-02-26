using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Faculties.Responses;

public record UpdateFacultyRequestResponse : RequestResponseBase
{
    public bool Updated { get; set; }

    public override object? GetPayload() => Updated;
}
