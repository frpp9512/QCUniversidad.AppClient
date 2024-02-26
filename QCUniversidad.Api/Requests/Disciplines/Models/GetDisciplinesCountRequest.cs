using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class GetDisciplinesCountRequest : RequestBase<GetDepartmentDisciplinesCountRequestResponse> { }
