using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Requests.Faculties.Responses;

public record CreateFacultyRequestResponse : CreatedRequestResponseBase<Guid, FacultyDto> { }
