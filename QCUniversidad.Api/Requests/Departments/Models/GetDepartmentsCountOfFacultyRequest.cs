using MediatR;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class GetDepartmentsCountOfFacultyRequest : IRequest<GetDepartmentsCountOfFacultyRequestResponse>
{
    public Guid FacultyId { get; set; }
}
