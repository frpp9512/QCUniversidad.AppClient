using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class ExistCourseHandler(ICoursesManager coursesManager) : IRequestHandler<ExistCourseRequest, ExistsCourseResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;

    public async Task<ExistsCourseResponse> Handle(ExistCourseRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _coursesManager.ExistsCourseAsync(request.CourseId);
            return new()
            {
                CourseId = request.CourseId,
                ExistCourse = result
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [ex.Message],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
