using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class GetCoursesBySchoolYearAndCareerOfFacultyHandler(ICoursesManager coursesManager,
                                                             ICareersManager careersManager,
                                                             ISchoolYearsManager schoolYearsManager,
                                                             IFacultiesManager facultiesManager,
                                                             IMapper mapper) : IRequestHandler<GetCoursesBySchoolYearAndCareerOfFacultyRequest, GetCoursesBySchoolYearAndCareerOfFacultyResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly ICareersManager _careersManager = careersManager;
    private readonly ISchoolYearsManager _schoolYearsManager = schoolYearsManager;
    private readonly IFacultiesManager _facultiesManager = facultiesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCoursesBySchoolYearAndCareerOfFacultyResponse> Handle(GetCoursesBySchoolYearAndCareerOfFacultyRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _careersManager.ExistsCareerAsync(request.CareerId))
            {
                return new()
                {
                    ErrorMessages = [$"The career with id {request.CareerId} doesn't exists."]
                };
            }

            if (!await _schoolYearsManager.ExistSchoolYearAsync(request.SchoolYearId))
            {
                return new()
                {
                    ErrorMessages = [$"The school year with id {request.SchoolYearId} doesn't exists."]
                };
            }

            if (!await _facultiesManager.ExistFacultyAsync(request.FacultyId))
            {
                return new()
                {
                    ErrorMessages = [$"The faculty with id {request.FacultyId} doesn't exists."]
                };
            }

            var result = await _coursesManager.GetCoursesAsync(request.CareerId, request.SchoolYearId, request.FacultyId);
            var dtos = result.Select(_mapper.Map<CourseDto>).OrderBy(dto => dto.CareerId).ThenBy(dto => dto.CareerYear).ToList();
            return new()
            {
                CareerId = request.CareerId,
                FacultyId = request.FacultyId,
                SchoolYearId = request.SchoolYearId,
                Courses = dtos
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [ex.Message]
            };
        }
    }
}
