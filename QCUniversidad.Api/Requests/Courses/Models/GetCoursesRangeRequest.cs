using MediatR;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class GetCoursesRangeRequest : IRequest<GetCoursesRangeRequestResponse>
{
    public int From { get; set; }
    public int To { get; set; }
}
