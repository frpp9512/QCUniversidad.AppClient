using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class ExistCourseRequestHandler(ICoursesManager coursesManager) : IRequestHandler<ExistCourseRequest, ExistsCourseRequestResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;

    public async Task<ExistsCourseRequestResponse> Handle(ExistCourseRequest request, CancellationToken cancellationToken)
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
