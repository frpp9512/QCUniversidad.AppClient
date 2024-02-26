using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class GetCoursesBySchoolYearOfFacultyHandler(ICoursesManager coursesManager,
                                                    ISchoolYearsManager schoolYearsManager,
                                                    IFacultiesManager facultiesManager,
                                                    IMapper mapper) : IRequestHandler<GetCoursesBySchoolYearOfFacultyRequest, GetCoursesBySchoolYearOfFacultyRequestResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly ISchoolYearsManager _schoolYearsManager = schoolYearsManager;
    private readonly IFacultiesManager _facultiesManager = facultiesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCoursesBySchoolYearOfFacultyRequestResponse> Handle(GetCoursesBySchoolYearOfFacultyRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _schoolYearsManager.ExistSchoolYearAsync(request.SchoolYearId))
            {
                return new()
                {
                    RequestId = request.RequestId,
                    ErrorMessages = [$"The school year with id {request.SchoolYearId} doesn't exists."],
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            if (!await _facultiesManager.ExistFacultyAsync(request.FacultyId))
            {
                return new()
                {
                    RequestId = request.RequestId,
                    ErrorMessages = [$"The faculty with id {request.FacultyId} doesn't exists."],
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            IList<CourseModel> result = await _coursesManager.GetCoursesAsync(request.SchoolYearId, request.FacultyId);
            var dtos = result.Select(_mapper.Map<CourseDto>).OrderBy(dto => dto.CareerId).ThenBy(dto => dto.CareerYear).ToList();
            return new()
            {
                RequestId = request.RequestId,
                SchoolYearId = request.SchoolYearId,
                FacultyId = request.FacultyId,
                Courses = dtos
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [ex.Message],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
