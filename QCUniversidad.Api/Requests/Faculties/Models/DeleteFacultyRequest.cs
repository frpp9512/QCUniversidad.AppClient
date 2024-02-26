using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Faculties.Responses;

namespace QCUniversidad.Api.Requests.Faculties.Models;

public class DeleteFacultyRequest : RequestBase<DeleteFacultyRequestResponse>
{
    public Guid FacultyId { get; set; }
}
