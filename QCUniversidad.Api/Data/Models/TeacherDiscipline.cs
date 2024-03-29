﻿namespace QCUniversidad.Api.Data.Models;

/// <summary>
/// Represents the relationship between a teacher and a discipline.
/// </summary>
public class TeacherDiscipline
{
    public Guid TeacherId { get; set; }
    public TeacherModel? Teacher { get; set; }
    public Guid DisciplineId { get; set; }
    public DisciplineModel? Discipline { get; set; }
}
