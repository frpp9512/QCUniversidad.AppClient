using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Requests.Curriculums.Responses;

public record GetCurriculumsForCareerResponse : RequestResponseBase
{
    public Guid CareerId { get; set; }
    public List<CurriculumDto>? CareerCurriculums { get; set; }

    public override object? GetPayload() => CareerCurriculums;
}
