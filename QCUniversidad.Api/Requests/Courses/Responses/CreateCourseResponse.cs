using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Responses;

public record CreateCourseResponse : CreatedRequestResponseBase<Guid, CourseDto>
{
}
