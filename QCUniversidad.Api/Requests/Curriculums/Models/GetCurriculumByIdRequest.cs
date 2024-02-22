using MediatR;
using QCUniversidad.Api.Requests.Curriculums.Responses;

namespace QCUniversidad.Api.Requests.Curriculums.Models;

public class GetCurriculumByIdRequest : IRequest<GetCurriculumByIdResponse>
{
    public Guid CurriculumId { get; set; }
}
