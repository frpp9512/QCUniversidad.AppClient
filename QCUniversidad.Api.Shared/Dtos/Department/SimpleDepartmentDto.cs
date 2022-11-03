﻿using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Shared.Dtos.Department;

public record SimpleDepartmentDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? InternalId { get; set; }
    public int DisciplinesCount { get; set; }
    public Guid FacultyId { get; set; }
    public FacultyDto? Faculty { get; set; }
    public double? TotalTimeFund { get; set; }
    public double? Load { get; set; }
    public double? LoadPercent { get; set; }
    public double? LoadCovered { get; set; }
    public double? LoadCoveredPercent { get; set; }
}
