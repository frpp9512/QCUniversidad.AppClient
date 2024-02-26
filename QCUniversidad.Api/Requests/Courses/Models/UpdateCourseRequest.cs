using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Courses.Responses;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class UpdateCourseRequest : RequestBase<UpdateCourseRequestResponse>
{
    public EditCourseDto? CourseToUpdate { get; set; }
}
