using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class GetCoursesRangeRequestHandler(ICoursesManager coursesManager, IMapper mapper) : IRequestHandler<GetCoursesRangeRequest, GetCoursesRangeRequestResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCoursesRangeRequestResponse> Handle(GetCoursesRangeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<CourseModel> courses = await _coursesManager.GetCoursesAsync(request.From, request.To);
            var dtos = courses.Select(_mapper.Map<CourseDto>).ToList();
            return new()
            {
                From = request.From,
                To = request.To,
                CoursesRange = dtos
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [ ex.Message ],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
