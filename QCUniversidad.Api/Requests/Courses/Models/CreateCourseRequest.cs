﻿using MediatR;
using QCUniversidad.Api.Requests.Courses.Responses;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.Requests.Courses.Models;

public class CreateCourseRequest : IRequest<CreateCourseRequestResponse>
{
    public required NewCourseDto NewCourse { get; set; }
}