using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class UpdateCourseRequestHandler(ICoursesManager coursesManager, IMapper mapper) : IRequestHandler<UpdateCourseRequest, UpdateCourseRequestResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateCourseRequestResponse> Handle(UpdateCourseRequest request, CancellationToken cancellationToken)
    {
        if (request.CourseToUpdate is null)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Must be provided a course data."]
            };
        }

        try
        {
            bool result = await _coursesManager.UpdateCourseAsync(_mapper.Map<CourseModel>(request.CourseToUpdate));
            return !result
                ? new()
                {
                    RequestId = request.RequestId,
                    ErrorMessages = [$"The course couldn't be updated."],
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                }
                : new()
                {
                    RequestId = request.RequestId,
                    UpdatedCourse = request.CourseToUpdate
                };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while updating the course. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
