using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class ExistCourseByCareerYearAndModalityHandler(ICareersManager careersManager,
                                                       ICoursesManager coursesManager) : IRequestHandler<ExistCourseByCareerYearAndModalityRequest, ExistCourseByCareerYearAndModalityResponse>
{
    private readonly ICareersManager _careersManager = careersManager;
    private readonly ICoursesManager _coursesManager = coursesManager;

    public async Task<ExistCourseByCareerYearAndModalityResponse> Handle(ExistCourseByCareerYearAndModalityRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _careersManager.ExistsCareerAsync(request.CareerId))
            {
                return new()
                {
                    ErrorMessages = [$"The career with id: {request.CareerId} doesn't exists."],
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            if (request.CareerYear < 0)
            {
                return new()
                {
                    ErrorMessages = [$"The career year has an invalid value ({request.CareerYear})."],
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            bool result = await _coursesManager.CheckCourseExistenceByCareerYearAndModality(request.CareerId, request.CareerYear, request.Modality);
            return new()
            {
                CareerId = request.CareerId,
                TeachingModality = request.Modality,
                CareerYear = request.CareerYear,
                Exists = result
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
