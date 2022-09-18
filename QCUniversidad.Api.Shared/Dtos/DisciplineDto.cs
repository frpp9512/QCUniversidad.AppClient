﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos
{
    public record DisciplineDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int TeachersCount { get; set; }
        public int SubjectsCount { get; set; }
        public Guid DepartmentId { get; set; }
        public DepartmentDto Department { get; set; }
    }
}
