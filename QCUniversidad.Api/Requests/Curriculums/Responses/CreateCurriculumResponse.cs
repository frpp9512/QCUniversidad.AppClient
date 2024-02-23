using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Requests.Curriculums.Responses;

public record CreateCurriculumResponse : CreatedRequestResponseBase<Guid, CurriculumDto> { }
