using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class ExistCourseByCareerYearAndModalityRequestHandler(ICareersManager careersManager,
                                                       ICoursesManager coursesManager) : IRequestHandler<ExistCourseByCareerYearAndModalityRequest, ExistCourseByCareerYearAndModalityRequestResponse>
{
    private readonly ICareersManager _careersManager = careersManager;
    private readonly ICoursesManager _coursesManager = coursesManager;

    public async Task<ExistCourseByCareerYearAndModalityRequestResponse> Handle(ExistCourseByCareerYearAndModalityRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _careersManager.ExistsCareerAsync(request.CareerId))
            {
                return new()
                {
                    RequestId = request.RequestId,
                    ErrorMessages = [$"The career with id: {request.CareerId} doesn't exists."],
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            if (request.CareerYear < 0)
            {
                return new()
                {
                    RequestId = request.RequestId,
                    ErrorMessages = [$"The career year has an invalid value ({request.CareerYear})."],
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            bool result = await _coursesManager.CheckCourseExistenceByCareerYearAndModality(request.CareerId, request.CareerYear, request.Modality);
            return new()
            {
                RequestId = request.RequestId,
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
                RequestId = request.RequestId,
                ErrorMessages = [ex.Message],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
