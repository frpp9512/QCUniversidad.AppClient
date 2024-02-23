﻿using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.Requests.Careers.Responses;

public record GetCareerByIdRequestResponse : RequestResponseBase
{
    public CareerDto? Career { get; set; }

    public override object? GetPayload() => Career;
}