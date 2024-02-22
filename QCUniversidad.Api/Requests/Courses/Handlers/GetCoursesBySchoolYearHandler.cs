using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class GetCoursesBySchoolYearHandler(ICoursesManager coursesManager, ISchoolYearsManager schoolYearsManager, IMapper mapper) : IRequestHandler<GetCoursesBySchoolYearRequest, GetCoursesBySchoolYearResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly ISchoolYearsManager _schoolYearsManager = schoolYearsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCoursesBySchoolYearResponse> Handle(GetCoursesBySchoolYearRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _schoolYearsManager.ExistSchoolYearAsync(request.SchoolYearId))
            {
                return new()
                {
                    ErrorMessages = [ $"The school year with id {request.SchoolYearId} doesn't exist." ],
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            IList<CourseModel> result = await _coursesManager.GetCoursesAsync(request.SchoolYearId);
            var dtos = result.Select(_mapper.Map<CourseDto>).OrderBy(dto => dto.CareerId).ThenBy(dto => dto.CareerYear).ToList();
            return new()
            {
                SchoolYearId = request.SchoolYearId,
                Courses = dtos
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
