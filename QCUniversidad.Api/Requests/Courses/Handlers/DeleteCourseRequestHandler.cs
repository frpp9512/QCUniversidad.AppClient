using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class DeleteCourseRequestHandler(ICoursesManager coursesManager) : IRequestHandler<DeleteCourseRequest, DeleteCourseRequestResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;

    public async Task<DeleteCourseRequestResponse> Handle(DeleteCourseRequest request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _coursesManager.DeleteCourseAsync(request.CourseId);
            return result ? new()
            {
                Deleted = true
            } : new()
            {
                ErrorMessages = [$"The course with id: {request.CourseId} couldn't be removed."],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
        catch (CourseNotFoundException)
        {
            return new()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessages = [$"The course with id: {request.CourseId} was not found."]
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ErrorMessages = [$"Error while trying to delete the course with id {request.CourseId}. Error message: {ex.Message}"]
            };
        }
    }
}
