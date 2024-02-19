﻿using MediatR;
using QCUniversidad.Api.Requests.Curriculums.Responses;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Requests.Curriculums.Models;

public class UpdateCurriculumRequest : IRequest<UpdateCurriculumResponse>
{
    public EditCurriculumDto? CurriculumToUpdate { get; set; }
}
