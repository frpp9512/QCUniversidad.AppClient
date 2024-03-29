﻿using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Dtos.LoadItem;
using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.Api.Shared.Dtos.Subject;
using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Shared.Dtos.TeachingPlan;

public record TeachingPlanItemDto
{
    public Guid Id { get; set; }
    public Guid SubjectId { get; set; }
    public SubjectDto? Subject { get; set; }
    public TeachingActivityType Type { get; set; }
    public double HoursPlanned { get; set; }
    public uint GroupsAmount { get; set; }
    public double TotalHoursPlanned { get; set; }
    public bool FromPostgraduateCourse { get; set; }
    public bool IsNotLoadGenerator { get; set; }
    public double TotalLoadCovered { get; set; }
    public double LoadCoveredPercent => Math.Round(TotalLoadCovered / TotalHoursPlanned * 100, 1);
    public bool AllowLoad { get; set; }
    public Guid CourseId { get; set; }
    public required SimpleCourseDto Course { get; set; }
    public Guid PeriodId { get; set; }
    public required SimplePeriodDto Period { get; set; }
    public required IList<SimpleLoadItemDto> LoadItems { get; set; }
}