using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;
using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Dtos.Period;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class GetCoursePlanningInfoForPeriodRequestHandler(ICoursesManager coursesManager,
                                                          IPeriodsManager periodsManager,
                                                          IMapper mapper) : IRequestHandler<GetCoursePlanningInfoForPeriodRequest, GetCoursePlanningInfoForPeriodRequestResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;
    private readonly IPeriodsManager _periodsManager = periodsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCoursePlanningInfoForPeriodRequestResponse> Handle(GetCoursePlanningInfoForPeriodRequest request, CancellationToken cancellationToken)
    {
        try
        {
            CourseModel course = await _coursesManager.GetCourseAsync(request.CourseId);
            PeriodModel period = await _periodsManager.GetPeriodAsync(request.PeriodId);
            double totalPlanned = await _coursesManager.GetTotalHoursInPeriodForCourseAsync(request.CourseId, request.PeriodId);
            double realPlanned = await _coursesManager.GetRealHoursPlannedInPeriodForCourseAsync(request.CourseId, request.PeriodId);
            CoursePeriodPlanningInfoDto dto = new()
            {
                PeriodId = request.PeriodId,
                Period = _mapper.Map<SimplePeriodDto>(period),
                CourseId = request.CourseId,
                Course = _mapper.Map<SimpleCourseDto>(course),
                RealHoursPlanned = realPlanned,
                TotalHoursPlanned = totalPlanned
            };
            return new()
            {
                PlanningInfo = dto
            };
        }
        catch (ArgumentNullException ex)
        {
            return new()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ErrorMessages = [$"Error with request argument. Error message: {ex.Message}"]
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
        catch (PeriodNotFoundException)
        {
            return new()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessages = [$"The period with id: {request.PeriodId} was not found."]
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ErrorMessages = [$"Error while trying to get the planning info of course: {request.CourseId} in the period: {request.PeriodId}. Error message: {ex.Message}"]
            };
        }
    }
}
