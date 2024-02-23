﻿using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record GetDepartmentByIdRequestResponse : RequestResponseBase
{
    public Guid DepartmentId { get; set; }
    public DepartmentDto? Department { get; set; }

    public override object? GetPayload() => Department;
}