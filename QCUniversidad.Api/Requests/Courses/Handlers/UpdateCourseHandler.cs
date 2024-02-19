using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class UpdateCourseHandler(ICoursesManager coursesManager, IMapper mapper) : IRequestHandler<UpdateCourseRequest, UpdateCourseResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateCourseResponse> Handle(UpdateCourseRequest request, CancellationToken cancellationToken)
    {
        if (request.CourseToUpdate is null)
        {
            return new() { ErrorMessages = [$"Must be provided a course data."] };
        }

        try
        {
            bool result = await _coursesManager.UpdateCourseAsync(_mapper.Map<CourseModel>(request.CourseToUpdate));
            return !result
                ? new()
                {
                    ErrorMessages = [$"The course couldn't be updated."]
                }
                : new() { UpdatedCourse = request.CourseToUpdate };
        }
        catch (Exception ex)
        {
            return new() { ErrorMessages = [$"Error while updating the course. Error message: {ex.Message}"] };
        }
    }
}
