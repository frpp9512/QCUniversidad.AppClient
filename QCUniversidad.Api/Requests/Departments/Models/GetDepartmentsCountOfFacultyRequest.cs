using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentsCountOfFacultyRequest : IRequest<GetDepartmentsCountOfFacultyResponse>
{
    public Guid FacultyId { get; set; }
}
