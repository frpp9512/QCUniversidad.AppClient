﻿using QCUniversidad.Api.Shared.Dtos.Career;
using QCUniversidad.Api.Shared.Dtos.Curriculum;
using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.Api.Shared.Dtos.SchoolYear;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.Course;

public record SimpleCourseDto
{
    public Guid Id { get; set; }
    public Guid SchoolYearId { get; set; }
    public SimpleSchoolYearDto SchoolYear { get; set; }
    public int CareerYear { get; set; }
    public string Denomination { get; set; }
    public TeachingModality TeachingModality { get; set; }
    public uint Enrolment { get; set; }
    public Guid CareerId { get; set; }
    public CareerDto Career { get; set; }
    public Guid CurriculumId { get; set; }
    public CurriculumDto Curriculum { get; set; }
}