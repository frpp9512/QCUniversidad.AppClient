using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentsOfFacultyRequest : RequestBase<GetDepartmentsOfFacultyRequestResponse>
{
    public Guid FacultyId { get; set; }
}
