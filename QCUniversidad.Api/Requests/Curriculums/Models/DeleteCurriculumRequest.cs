using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;

namespace QCUniversidad.Api.Requests.Curriculums.Models;

public class DeleteCurriculumRequest : RequestBase<DeleteCurriculumRequestResponse>
{
    public Guid CurriculumId { get; set; }
}
