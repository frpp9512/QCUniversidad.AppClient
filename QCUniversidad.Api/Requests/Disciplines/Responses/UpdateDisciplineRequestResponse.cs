using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Disciplines.Responses;

public record UpdateDisciplineRequestResponse : RequestResponseBase
{
    public bool Updated { get; set; }

    public override object? GetPayload() => Updated;
}
