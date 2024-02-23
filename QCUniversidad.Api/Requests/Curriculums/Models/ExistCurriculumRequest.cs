using MediatR;
using QCUniversidad.Api.Requests.Curriculums.Responses;

namespace QCUniversidad.Api.Requests.Curriculums.Models;

public class ExistCurriculumRequest : IRequest<ExistCurriculumRequestResponse>
{
    public Guid CurriculumId { get; set; }
}
