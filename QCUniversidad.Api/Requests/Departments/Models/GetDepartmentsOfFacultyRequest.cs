using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentsOfFacultyRequest : IRequest<GetDepartmentsOfFacultyRequestResponse>
{
    public Guid FacultyId { get; set; }
}
