﻿using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Models;

public class ExistDepartmentRequest : RequestBase<ExistDepartmentRequestResponse>
{
    public Guid DepartmentId { get; set; }
}
