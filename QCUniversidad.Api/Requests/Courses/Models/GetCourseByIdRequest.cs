using MediatR;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class GetCourseByIdRequest : IRequest<GetCourseByIdRequestResponse>
{
    public Guid CourseId { get; set; }
}
