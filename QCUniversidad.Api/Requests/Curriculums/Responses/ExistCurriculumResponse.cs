﻿using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Requests.Curriculums.Responses;

public record ExistCurriculumResponse : ResponseBase
{
    public bool ExistCurriculum { get; set; }

    public override object? GetPayload() => ExistCurriculum;
}
