using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class GetCourseByIdRequestHandler(ICoursesManager coursesManager, IMapper mapper) : IRequestHandler<GetCourseByIdRequest, GetCourseByIdRequestResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCourseByIdRequestResponse> Handle(GetCourseByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            CourseModel result = await _coursesManager.GetCourseAsync(request.CourseId);
            CourseDto dto = _mapper.Map<CourseDto>(result);
            return new()
            {
                CourseId = request.CourseId,
                Course = dto
            };
        }
        catch (CourseNotFoundException)
        {
            return new()
            {
                ErrorMessages = [$"The course with id: {request.CourseId} doesn't exists."],
                StatusCode = System.Net.HttpStatusCode.NotFound
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
