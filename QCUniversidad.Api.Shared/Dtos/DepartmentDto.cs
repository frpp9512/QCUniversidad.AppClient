﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos
{
    public record DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisciplinesCount { get; set; }
        public Guid FacultyId { get; set; }
        public FacultyDto Faculty { get; set; }
    }
}
