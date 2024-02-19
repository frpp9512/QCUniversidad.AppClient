﻿using MediatR;
using QCUniversidad.Api.Requests.Curriculums.Responses;

namespace QCUniversidad.Api.Requests.Curriculums.Models;

public class GetCurriculumsCountRequest : IRequest<GetCurriculumsCountResponse> { }
