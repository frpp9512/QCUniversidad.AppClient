using MediatR;
using QCUniversidad.Api.Requests.Disciplines.Responses;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.Requests.Disciplines.Models;

public class UpdateDisciplineRequest : IRequest<UpdateDisciplineRequestResponse>
{
    public EditDisciplineDto? DisciplineToUpdate { get; set; }
}
