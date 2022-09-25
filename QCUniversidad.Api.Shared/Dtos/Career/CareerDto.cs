﻿using QCUniversidad.Api.Shared.Dtos.Faculty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Career
{
    public record CareerDto : EditCareerDto
    {
        public FacultyDto Faculty { get; set; }
    }
}