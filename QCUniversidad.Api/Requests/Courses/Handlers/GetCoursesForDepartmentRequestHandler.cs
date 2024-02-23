using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class GetCoursesForDepartmentRequestHandler(ICoursesManager coursesManager, IMapper mapper) : IRequestHandler<GetCoursesForDepartmentRequest, GetCoursesForDepartmentRequestResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCoursesForDepartmentRequestResponse> Handle(GetCoursesForDepartmentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<CourseModel> result = await _coursesManager.GetCoursesForDepartmentAsync(request.DepartmentId, request.SchoolYearId);
            var dtos = result.Select(_mapper.Map<CourseDto>).ToList();
            return new() { Courses = dtos };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [ $"Error while loading the courses for department: {request.DepartmentId} in the school year: {request.SchoolYearId}. Error message: {ex.Message}" ],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
