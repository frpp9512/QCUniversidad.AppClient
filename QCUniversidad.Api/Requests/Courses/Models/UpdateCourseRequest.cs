using MediatR;
using QCUniversidad.Api.Requests.Courses.Responses;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class UpdateCourseRequest : IRequest<UpdateCourseRequestResponse>
{
    public EditCourseDto? CourseToUpdate { get; set; }
}
