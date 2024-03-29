﻿namespace QCUniversidad.Api.Data.Models;

/// <summary>
/// A career managed by the faculty.
/// </summary>
public record CareerModel
{
    /// <summary>
    /// Primary key value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the career. Example: Industrial Engineering.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The description of the career. Example: Industrial Engineering is an engineering profession that is concerned with the optimization of complex processes, systems, or organizations by developing, improving and implementing integrated systems of people, money, knowledge, information and equipment.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Defineśif the career is a postgraduate course.
    /// </summary>
    public bool PostgraduateCourse { get; set; }

    /// <summary>
    /// The set of curriculums intended for the career.
    /// </summary>
    public required IList<CurriculumModel> Curricula { get; set; }

    /// <summary>
    /// The school years of the career.
    /// </summary>
    public required IList<CourseModel> Courses { get; set; }

    /// <summary>
    /// The identifier of the Faculty that manages the career.
    /// </summary>
    public Guid FacultyId { get; set; }

    /// <summary>
    /// The faculty that manages the career.
    /// </summary>
    public required FacultyModel Faculty { get; set; }

    /// <summary>
    /// The relation of departments that attend the career.
    /// </summary>
    public required IList<DepartmentCareer> CareerDepartments { get; set; }
}