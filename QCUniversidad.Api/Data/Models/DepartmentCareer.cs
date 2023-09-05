namespace QCUniversidad.Api.Data.Models;

/// <summary>
/// Represents the relationship between departments and creeers.
/// </summary>
public record DepartmentCareer
{
    /// <summary>
    /// The primary key value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The id of the department related with the career.
    /// </summary>
    public Guid DepartmentId { get; set; }

    /// <summary>
    /// The department related with the career.
    /// </summary>
    public required DepartmentModel Department { get; set; }

    /// <summary>
    /// The id of the career related with the department.
    /// </summary>
    public Guid CareerId { get; set; }

    /// <summary>
    /// The career related with the department.
    /// </summary>
    public required CareerModel Career { get; set; }
}