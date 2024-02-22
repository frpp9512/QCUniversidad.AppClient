using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentsOfFacultyRequest : IRequest<GetDepartmentsOfFacultyResponse>
{
    public Guid FacultyId { get; set; }
}
