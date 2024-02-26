using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class GetCoursesRangeRequest : RequestBase<GetCoursesRangeRequestResponse>
{
    public int From { get; set; }
    public int To { get; set; }
}
