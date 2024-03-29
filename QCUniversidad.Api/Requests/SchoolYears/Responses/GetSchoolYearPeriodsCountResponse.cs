﻿using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.SchoolYears.Responses;

public record GetSchoolYearPeriodsCountResponse : RequestResponseBase
{
    public Guid SchoolYearId { get; set; }
    public int PeriodsCount { get; set; }

    public override object? GetPayload() => PeriodsCount;
}
