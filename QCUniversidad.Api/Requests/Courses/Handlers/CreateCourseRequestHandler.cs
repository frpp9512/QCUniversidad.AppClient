using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class CreateCourseRequestHandler(ICoursesManager coursesManager, IMapper mapper) : IRequestHandler<CreateCourseRequest, CreateCourseRequestResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateCourseRequestResponse> Handle(CreateCourseRequest request, CancellationToken cancellationToken)
    {
        try
        {
            CourseModel model = _mapper.Map<CourseModel>(request.NewCourse);
            var createdCourse = await _coursesManager.CreateCourseAsync(model);
            return new CreateCourseRequestResponse()
            {
                RequestId = request.RequestId,
                CreatedId = createdCourse.Id,
                CreatedEntity = _mapper.Map<CourseDto>(createdCourse),
                ApiEntityEndpointAction = "GetById"
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [ ex.Message ],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
