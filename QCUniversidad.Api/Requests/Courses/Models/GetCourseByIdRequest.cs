using MediatR;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class GetCourseByIdRequest : IRequest<GetCourseByIdResponse>
{
    public Guid CourseId { get; set; }
}
