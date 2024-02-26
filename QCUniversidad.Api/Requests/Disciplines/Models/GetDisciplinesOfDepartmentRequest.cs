using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class GetDisciplinesOfDepartmentRequest : RequestBase<GetDisciplinesOfDepartmentRequestResponse>
{
    public Guid DepartmentId { get; set; }
}
