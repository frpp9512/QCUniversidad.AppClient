﻿using MediatR;
using QCUniversidad.Api.Requests.Courses.Responses;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class ExistCourseRequest : IRequest<ExistsCourseRequestResponse>
{
    public Guid CourseId { get; set; }
}