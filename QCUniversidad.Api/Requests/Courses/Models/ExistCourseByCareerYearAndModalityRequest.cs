using MediatR;
using QCUniversidad.Api.Requests.Courses.Responses;
using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class ExistCourseByCareerYearAndModalityRequest : IRequest<ExistCourseByCareerYearAndModalityResponse>
{
    public Guid CareerId { get; set; }
    public int CareerYear { get; set; }
    public TeachingModality Modality { get; set; }
}
