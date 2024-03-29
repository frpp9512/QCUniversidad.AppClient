﻿using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class ExistCourseRequest : RequestBase<ExistsCourseRequestResponse>
{
    public Guid CourseId { get; set; }
}
