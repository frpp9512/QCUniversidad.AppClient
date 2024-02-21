﻿using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Departments.Responses;

public record GetDepartmentDisciplinesCountResponse : ResponseBase
{
    public int Count { get; set; }

    public override object? GetPayload() => Count;
}
