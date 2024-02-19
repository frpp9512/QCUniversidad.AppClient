using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Requests.Courses.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Handlers;

public class GetCoursesCountHandler(ICoursesManager coursesManager) : IRequestHandler<GetCoursesCountRequest, GetCoursesCountResponse>
{
    private readonly ICoursesManager _coursesManager = coursesManager;

    public async Task<GetCoursesCountResponse> Handle(GetCoursesCountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            int count = await _coursesManager.GetCoursesCountAsync();
            return new() { CoursesCount = count };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [ ex.Message ]
            };
        }
    }
}
