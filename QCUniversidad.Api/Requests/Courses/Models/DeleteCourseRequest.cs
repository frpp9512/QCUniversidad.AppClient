using MediatR;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class DeleteCourseRequest : IRequest<DeleteCourseRequestResponse>
{
    public Guid CourseId { get; set; }
}
