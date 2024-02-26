using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Disciplines.Responses;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class CreateDisciplineRequest : RequestBase<CreateDisciplineRequestResponse>
{
    public NewDisciplineDto? NewDiscipline { get; set; }
}
