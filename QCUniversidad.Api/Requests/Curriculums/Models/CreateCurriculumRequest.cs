using MediatR;
using QCUniversidad.Api.Requests.Curriculums.Responses;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Requests.Curriculums.Models;

public class CreateCurriculumRequest : IRequest<CreateCurriculumResponse>
{
    public NewCurriculumDto? NewCurriculum { get; set; }
}
