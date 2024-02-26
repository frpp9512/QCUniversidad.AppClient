using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Faculties.Responses;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Requests.Faculties.Models;

public class UpdateFacultyRequest : RequestBase<UpdateFacultyRequestResponse>
{
    public FacultyDto? Faculty { get; set; }
}
