using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Requests.Curriculums.Models;

public class CreateCurriculumRequest : RequestBase<CreateCurriculumRequestResponse>
{
    public NewCurriculumDto? NewCurriculum { get; set; }
}
