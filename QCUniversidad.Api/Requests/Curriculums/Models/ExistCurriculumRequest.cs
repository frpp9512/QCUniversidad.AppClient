﻿using MediatR;
using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;

namespace QCUniversidad.Api.Requests.Curriculums.Models;

public class ExistCurriculumRequest : RequestBase<ExistCurriculumRequestResponse>
{
    public Guid CurriculumId { get; set; }
}
