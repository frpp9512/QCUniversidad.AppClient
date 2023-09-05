namespace QCUniversidad.Api.Data.Models;

/// <summary>
/// Reprents a Faculty of an University, that can manage a set of careers and a set of teachers.
/// </summary>
public record FacultyModel
{
    /// <summary>
    /// Primary key value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the faculty. Example: Ingeniería Industrial
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The campus where the faculty belongs to. Example: Oscar Lucero Moya
    /// </summary>
    public string? Campus { get; set; }

    /// <summary>
    /// The internat management identifier. Example: 12
    /// </summary>
    public required string InternalId { get; set; }

    /// <summary>
    /// The set of departments the faculty have.
    /// </summary>
    public required IList<DepartmentModel> Deparments { get; set; }

    /// <summary>
    /// The careers that the faculty manages.
    /// </summary>
    public required IList<CareerModel> Carreers { get; set; }
}