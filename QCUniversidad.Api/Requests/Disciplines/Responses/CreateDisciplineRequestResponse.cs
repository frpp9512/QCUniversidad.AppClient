using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Requests.Disciplines.Responses;

public record CreateDisciplineRequestResponse : CreatedRequestResponseBase<Guid, SimpleDisciplineDto> { }
